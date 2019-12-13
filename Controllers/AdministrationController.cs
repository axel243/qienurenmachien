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
            model.Werkgevers.Add(new SelectListItem { Value = mockWerkgever.Id, Text = "Geen werkgever", Selected = true });
            foreach (var users in usersAreWerkgevers)
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

                model.Password = GetRandomPasswordUsingGUID(14) + "!";
                IdentityResult resultt = null;
                var result = await userManager.CreateAsync(user, model.Password);
                var role = await roleManager.FindByNameAsync(model.Role);
                resultt = await userManager.AddToRoleAsync(user, role.Name);
               
                if (result.Succeeded)
                {
                    if (resultt.Succeeded)
                    {
                        mailServer.SendRegisterUserMail(user.UserName, model.Password);
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

        public string GetRandomPasswordUsingGUID(int length)
        {
            // Get the GUID
            string guidResult = System.Guid.NewGuid().ToString();

            // Remove the hyphens
            guidResult = guidResult.Replace("-", string.Empty);
            guidResult = guidResult.Substring(0, 1).ToUpper() + guidResult.Substring(1);
            // Make sure length is valid
            if (length <= 0 || length > guidResult.Length)
                throw new ArgumentException("Length must be between 1 and " + guidResult.Length);

            // Return the first length bytes
            return guidResult.Substring(0, length);
        }


        public IActionResult ViewUser(string Id)
        {
            var singleuser = userManager.Users.Single(u => u.Id == Id);
            return View(singleuser);
        }

        public IActionResult ViewEmployer(string Id)
        {
            var singleuser = userManager.Users.Single(u => u.Id == Id);
            return View(singleuser);
        }

        public async Task<IActionResult> DeactivateUser(string Id)
        {
            var singleuser = await userManager.FindByIdAsync(Id);
            singleuser.ActiveUntil = DateTime.Now;
            var result = await userManager.UpdateAsync(singleuser);
            if (result.Succeeded)
            {
                return RedirectToAction("AdminDashboard", "Administration");
            }
            return View();
        }

        public async Task<IActionResult> TimeSheetOverview()
        {
            TimeSheetsViewModel model = new TimeSheetsViewModel { Month = DateTime.Now.ToString("MMMM"), theDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1) };
            model.Employees = await repo.GetAllEmployeeTimeSheets(model);
            model.Trainees = await repo.GetAllTraineeTimeSheets(model);
            model.Months = repo.GetMonths();
            model.Years = repo.GetYears();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> TimeSheetOverview(TimeSheetsViewModel model)
        {
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
                model.Months.Add(new SelectListItem { Value = "October", Text = "October" });
                model.Months.Add(new SelectListItem { Value = "September", Text = "September" });
                model.Months.Add(new SelectListItem { Value = "August", Text = "August" });
                model.Months.Add(new SelectListItem { Value = "July", Text = "July" });
                model.Months.Add(new SelectListItem { Value = "June", Text = "June" });
                model.Months.Add(new SelectListItem { Value = "May", Text = "May" });
                model.Months.Add(new SelectListItem { Value = "April", Text = "April" });
                model.Months.Add(new SelectListItem { Value = "March", Text = "March" });
                model.Months.Add(new SelectListItem { Value = "February", Text = "February" });
                model.Months.Add(new SelectListItem { Value = "January", Text = "January" });
            }
            model.Years = repo.GetYears();
            return View(model);
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