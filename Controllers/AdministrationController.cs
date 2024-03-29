using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QienUrenMachien.Entities;
using QienUrenMachien.Mail;
using QienUrenMachien.Models;
using QienUrenMachien.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QienUrenMachien.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ITimeSheetRepository repo;
        private readonly MailServer mailServer = new MailServer();

        public AdministrationController(RoleManager<IdentityRole> roleManager,
                                        UserManager<ApplicationUser> userManager,
                                        ITimeSheetRepository repo)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.repo = repo;
        }

        public async Task<IActionResult> AdminDashboard(string searchString)
        {
            //Het overzicht van alle werknemers en trainees. Gebruikt een viewmodel om de users op de delen per rol. 
            //Vervolgens moeten ze van list naar queryable worden geconverteerd, en weer terug om ze zoekbaar te maken.
            UsersViewModel model = new UsersViewModel();
            model.Employees = await userManager.GetUsersInRoleAsync("Werknemer");
            var employeesqueryable = model.Employees.AsQueryable();
            model.Trainees = await userManager.GetUsersInRoleAsync("Trainee");
            var traineesqueryable = model.Trainees.AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                employeesqueryable = employeesqueryable.Where(u => (u.UserName + u.Firstname + u.Lastname + u.City + u.Street).Contains(searchString, StringComparison.OrdinalIgnoreCase));
                model.Employees = employeesqueryable.ToList();
                traineesqueryable = traineesqueryable.Where(u => (u.UserName + u.Firstname + u.Lastname + u.City + u.Street).Contains(searchString, StringComparison.OrdinalIgnoreCase));
                model.Trainees = traineesqueryable.ToList();
            }

            return View(model);
        }

        public async Task<IActionResult> ViewEmployers(string searchString)
        {
            UsersViewModel model = new UsersViewModel();
            model.Employers = await userManager.GetUsersInRoleAsync("Werkgever");
            var employersqueryable = model.Employers.AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                employersqueryable = employersqueryable.Where(u => (u.UserName + u.Firstname + u.Lastname + u.City + u.Street).Contains(searchString, StringComparison.OrdinalIgnoreCase));
                model.Employers = employersqueryable.ToList();
            }

            return View(model);
        }

        [Route("Administration/RegisterUser")]
        [HttpGet]
        public async Task<IActionResult> RegisterUser()
        {

            RegisterViewModel model = new RegisterViewModel();
            await getAllWerkgevers(model);
            return View(model);
        }

        public async Task<List<SelectListItem>> getAllWerkgevers(RegisterViewModel model){
            var usersAreWerkgevers = await userManager.GetUsersInRoleAsync("Werkgever");
            var mockWerkgever = await userManager.FindByNameAsync("n457_n.8-93f5j3nls-f.e@gmail.com");
            model.Werkgevers = new List<SelectListItem>();
            model.Werkgevers.Add(new SelectListItem { Value = mockWerkgever.Id, Text = "Geen (HR @ Qien)", Selected = true });
            foreach (var users in usersAreWerkgevers.Where(u => u.ActiveUntil > DateTime.Now))
            {
                model.Werkgevers.Add(new SelectListItem() { Text = users.Firstname + " (Bedrijf: " + users.Lastname + ")", Value = users.Id });
            }
            return model.Werkgevers;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email,
                Firstname = model.Firstname,
                Lastname = model.Lastname,
                Street = model.Street,
                City = model.City,
                Zipcode = model.Zipcode,
                PhoneNumber = model.PhoneNumber,
                Country = model.Country,
                WerkgeverID = model.Werkgever,
                ProfileImageUrl = @"~/Uploads/Images/default_profile.jpg",
                ActiveFrom = DateTime.Now,
                ActiveUntil = DateTime.Now.AddYears(50)
                };

                if (model.Role == "Trainee"){
                    user.ActiveFrom = model.ActiveFrom;
                    user.ActiveUntil = model.ActiveFrom.AddYears(1); 
                }
                else if (model.Role == "Werknemer"){
                    user.ActiveFrom = model.ActiveFrom;
                }

                model.Password = GetRandomPasswordUsingGUID();
                IdentityResult resultt = null;
                var result = await userManager.CreateAsync(user, model.Password);
                var role = await roleManager.FindByNameAsync(model.Role);
                resultt = await userManager.AddToRoleAsync(user, role.Name);
               
                if (result.Succeeded)
                {
                    if (resultt.Succeeded)
                    {
                        if (model.Role != "Werkgever") {
                        mailServer.SendRegisterUserMail(user.UserName, model.Password, (user.Firstname +" " + user.Lastname));
                        }
                        return RedirectToAction("AdminDashboard", "Administration");

                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            else
            {
                await getAllWerkgevers(model);
            }
            return View(model);
        }

        public string GetRandomPasswordUsingGUID()
        {
 
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#&()�[{}]:;',?/*~$^+=<>";
            var capitals = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var nonalphas = "!@#&()�[{}]:;',?/*~$^+=<>";
            var numbers = "0123456789";
            var stringChars = new char[10];
            var random = new Random();

            var capital = capitals[random.Next(capitals.Length)];
            var nonalpha = nonalphas[random.Next(nonalphas.Length)];
            var number = numbers[random.Next(numbers.Length)];
            stringChars[0] = capital;
            stringChars[1] = nonalpha;
            stringChars[2] = number;
            for (int i = 3; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            return new string(stringChars); 
            
        }


        public IActionResult ViewUser(UserViewModel model, string Id)
        {
            model.theUser = userManager.Users.Single(u => u.Id == Id);
            model.ActiveUntilParam = model.theUser.ActiveUntil;
            return View(model);
        }

        public IActionResult ViewEmployer(UserViewModel model, string Id)
        {
            model.theUser = userManager.Users.Single(u => u.Id == Id);
            model.ActiveUntilParam = model.theUser.ActiveUntil;
            return View(model);
        }

        public async Task<IActionResult> DeactivateUser(UserViewModel model, string Id)
        {
            //De gebruiker deactiveren door de einddatum naar de nieuwe einddatum (die is gekozen door de admin) te veranderen.
            model.theUser = userManager.Users.Single(u => u.Id == Id);
            var singleuser = await userManager.FindByIdAsync(Id);
            singleuser.ActiveUntil = model.ActiveUntilParam;
            var result = await userManager.UpdateAsync(singleuser);
            if (result.Succeeded)
            {
                return RedirectToAction("AdminDashboard", "Administration");
            }
            return View();
        }

        public async Task<IActionResult> DeactivateEmployer(UserViewModel model, string Id)
        {
            model.theUser = userManager.Users.Single(u => u.Id == Id);
            var singleuser = await userManager.FindByIdAsync(Id);
            singleuser.ActiveUntil = model.ActiveUntilParam;
            var result = await userManager.UpdateAsync(singleuser);
            if (result.Succeeded)
            {
                return RedirectToAction("ViewEmployers", "Administration");
            }
            return View();
        }

        public async Task<IActionResult> TimeSheetOverview(string maand, DateTime jaar)
        {
            //Hier wordt een overzicht van alle timesheets per maand gemaakt, door een view-model te gebruiken waarin de users worden opgedeeld in hun rollen.
            //In dit view-model zit ook een lijst met maanden die in de repository dynamisch wordt gemaakt op basis van de huidige maand. Hetzelfde geld voor jaren.
            //Verder worden de huidige maand en het huidige jaar automatisch geselecteerd wanneer je het overzicht opent.
            if(maand == null)
            {
                maand = DateTime.Now.ToString("MMMM");
            }
            if(jaar.Year < 2000)
            {
                jaar = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            }
            TimeSheetsViewModel model = new TimeSheetsViewModel { Month = maand, theDate = jaar };
            model.Employees = await repo.GetAllEmployeeTimeSheets(model);
            model.Trainees = await repo.GetAllTraineeTimeSheets(model);
            model.Months = repo.GetMonths();
            model.Years = repo.GetYears();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> TimeSheetOverview(TimeSheetsViewModel model, string maand, DateTime jaar)
        {
            //Deze functie wordt aangeroepen zodra de gebruiker een selectie maand in de dropdown van maanden of jaren.
            //Als het geselecteerde jaar nog steeds het huidige jaar is, worden de maanden dynamisch aangemaakt. Als het niet het huidige jaar is, kan de gebruiker in de maanden-dropdown alle maanden kiezen.
            model.Employees = await repo.GetAllEmployeeTimeSheets(model);
            model.Trainees = await repo.GetAllTraineeTimeSheets(model);
            if (model.theDate.Year == DateTime.Now.Year)
            {
                model.Months = repo.GetMonths();
            } else
            {
                model.Months = new List<SelectListItem>();
                model.Months.Add(new SelectListItem { Value = "December", Text = "December" });
                model.Months.Add(new SelectListItem { Value = "November", Text = "November" });
                model.Months.Add(new SelectListItem { Value = "October", Text = "Oktober" });
                model.Months.Add(new SelectListItem { Value = "September", Text = "September" });
                model.Months.Add(new SelectListItem { Value = "August", Text = "Augustus" });
                model.Months.Add(new SelectListItem { Value = "July", Text = "Juli" });
                model.Months.Add(new SelectListItem { Value = "June", Text = "Juni" });
                model.Months.Add(new SelectListItem { Value = "May", Text = "Mei" });
                model.Months.Add(new SelectListItem { Value = "April", Text = "April" });
                model.Months.Add(new SelectListItem { Value = "March", Text = "Maart" });
                model.Months.Add(new SelectListItem { Value = "February", Text = "Februari" });
                model.Months.Add(new SelectListItem { Value = "January", Text = "Januari" });
            }
            model.Years = repo.GetYears();
            return RedirectToAction("TimeSheetOverview", "Administration", new { maand = model.Month, jaar = model.theDate });
            //return View(model);
        }

        public async Task<IActionResult> ShowUserTimeSheet(string Id, int SheetID)
        {
            var result = await repo.GetOneTimeSheetAsync(Id, SheetID);
            return View(result);
        }

        public async Task<IActionResult> ShowUserTimeSheetByUrl(string url)
        {
            var result = await repo.GetOneTimeSheetByUrl(url);
            return View(result);
        }


        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if(ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };

                IdentityResult result = await roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Administration");
                }

                foreach(IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }

            return View();
        }

        public IActionResult ListRoles()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            if(role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";
                return View("NotFound");
            }

            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };

            foreach (var user in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                };
            }

            return View(model);
        }

        // This action responds to HttpPost and receives EditRoleViewModel
        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                role.Name = model.RoleName;

                // Update the Role using UpdateAsync
                var result = await roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.roleId = roleId;

            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            };

            var model = new List<UserRoleViewModel>();

            foreach(var user in userManager.Users)
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if(await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }

                model.Add(userRoleViewModel);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }

            for (int i = 0; i < model.Count; i++)
            {
                var user = await userManager.FindByIdAsync(model[i].UserId);

                IdentityResult result = null;

                if (model[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && await userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                        return RedirectToAction("EditRole", new { Id = roleId });
                }
            }

            return RedirectToAction("EditRole", new { Id = roleId });
        }
    }
}