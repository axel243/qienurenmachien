using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using QienUrenMachien.Entities;
using QienUrenMachien.Hubs;
using QienUrenMachien.Mail;
using QienUrenMachien.Models;
using QienUrenMachien.Repositories;

namespace QienUrenMachien.Controllers
{
    public class ProfileController : Controller
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IActivityLogRepository repox;
        private readonly IHubContext<ChatHub> hub;
        private readonly IWebHostEnvironment env;
        private readonly MailServer mailServer;

        public ProfileController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IActivityLogRepository repox, IHubContext<ChatHub> hub, IWebHostEnvironment env)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.repox = repox;
            this.hub = hub;
            this.env = env;
            this.mailServer = new MailServer();
        }
        public IActionResult Index()
        {
            //userid wordt opgehaald van de gebruiker die ingelogd is
            var userid = userManager.GetUserId(HttpContext.User);

            if (userid == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                //de user wordt opgehaald middels de userid
                var user = userManager.FindByIdAsync(userid).Result;

                //user object wordt doorgegeven aan de view, waarmee de user gegevens worden gevuld in het profiel
                return View(@"~/Views/Account/Profile/Profile.cshtml", user);
            }
        }


        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            //user wordt opgehaald om de gegevens in te vullen op de pagina om de gegevens aan te passen


            //userid wordt opgehaald van ingelogde gebruiker en daarmee wordt zijn object opgehaald
            var userid = userManager.GetUserId(HttpContext.User);
            var currentUser = await userManager.FindByIdAsync(userid);

            if (currentUser == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userid} cannot be found";
                return View("NotFound");
            }

            //models converteren
            var currentUserModel = new ProfileViewModel
            {
                Id = currentUser.Id,
                UserName = currentUser.UserName,
                FirstName = currentUser.Firstname,
                LastName = currentUser.Lastname,
                PhoneNumber = currentUser.PhoneNumber,
                Street = currentUser.Street,
                Zipcode = currentUser.Zipcode,
                City = currentUser.City,
                Country = currentUser.Country,
                BankNumber = currentUser.BankNumber,
                ProfileImageUrl = currentUser.ProfileImageUrl
            };

            return View(@"~/Views/Account/Profile/EditProfile.cshtml", currentUserModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(ProfileViewModel model)
        {


            //userid wordt opgehaald uit de meegekregen model en daarmee wordt zijn object opgehaald
            var userid = model.Id;
            ApplicationUser currentUser = await userManager.FindByIdAsync(userid);

            var adminlist = await userManager.GetUsersInRoleAsync("Admin");

            if (currentUser == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userid} cannot be found";
                return View("NotFound");
            } 
            
            //als de gebruiker die zijn profiel aanpast een admin is, dan wordt het profiel meteen aangepast
            else if (adminlist.Contains(currentUser))
            {
                currentUser.UserName = model.UserName;
                currentUser.Firstname = model.FirstName;
                currentUser.Lastname = model.LastName;
                currentUser.Street = model.Street;
                currentUser.PhoneNumber = model.PhoneNumber;
                currentUser.Zipcode = model.Zipcode;
                currentUser.City = model.City;
                currentUser.Country = model.Country;
                currentUser.BankNumber = model.BankNumber;
                currentUser.ProfileImageUrl = model.ProfileImageUrl;
                currentUser.NewProfile = null;

                var resultt = await userManager.UpdateAsync(currentUser);

                if (resultt.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                return View(currentUser);
            }
            // als de gebruiker die zijn profiel aanpast geen admin is, dan wordt profiel wijziging in een wachtrij gezet ter goedkeuring van admin
            else
            {
                var fileNameHash = "";

                if (model.ProfileImage != null)
                {
                    UploadImage();
                }
                
                model.ProfileImageUrl = $@"~/Uploads/Images/" + fileNameHash;
                
                //het user model wordt geserialized naar jsonobject, in deze jsonobject zitten de nieuwe gegevens bij wijziging profiel
                var jsonProfile = JsonConvert.SerializeObject(model);

                //de property newprofile van huidige user krijgt invulling met deze jsonobject (potentieel nieuwe profiel)
                currentUser.NewProfile = jsonProfile;

                // Update the user using UpdateAsync
                var result = await userManager.UpdateAsync(currentUser);

                if (result.Succeeded)
                {
                    var activeUser = userManager.FindByIdAsync(userManager.GetUserId(HttpContext.User)).Result;
                    repox.LogActivity(activeUser, "EditProfile", $"{activeUser.Firstname[0]}. {activeUser.Lastname} heeft profiel verzoek ingediend.");

                    //laatste activiteit log wordt binnengehaald uit de entity
                    var lastActivity = repox.GetLastLog();

                    //call hub, hier wordt de websocket gebruikt om bericht te sturen naar de ontvanger dat met een nieuwe activiteitlog
                    await hub.Clients.All.SendAsync("ReceiveActivity", lastActivity);

                    var admins = await userManager.GetUsersInRoleAsync("Admin");

                    //een loop door de lijst admins, zodat iedere admin een mail ontvangt
                    foreach (ApplicationUser admin in admins)
                    {
                        //mail dat er een profielswijziging verzoek is
                        mailServer.SendEditedProfileMail(admin.UserName, admin.Firstname, currentUser.Firstname, currentUser.Lastname, currentUser.Id);
                    }

                    return View(@"~/Views/Account/Profile/StatusProfile.cshtml");
                }

                //methode om profiel foto te uploaden
                void UploadImage()
                {
                    var file = model.ProfileImage;
                    
                    var uploadPath = $@"wwwroot/Uploads/Images/";

                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    //uploaded file renamen met een hash
                    var fileExtension = Path.GetExtension(file.FileName);
                    var fileHash = Guid.NewGuid().ToString();
                    fileNameHash = fileHash + fileExtension;
                    
                    using (var fileStream = new FileStream(Path.Combine(uploadPath, fileNameHash), FileMode.Create, FileAccess.Write))
                    {
                        file.CopyTo(fileStream);
                    }
                }
                return View();
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> ConfirmProfile(string id)
        {
            //middels userid wordt user opgehaald
            var userid = id;
            var currentUser = await userManager.FindByIdAsync(userid);

            if (currentUser == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userid} cannot be found";
                return View(@"~/Views/Account/NotFound.cshtml");
            }

            //jsonobject wordt uit de property new profile gehaald
            var jsonProfile = currentUser.NewProfile;

            //huidige profiel zoals die is en was
            var currentProfile = new ProfileViewModel
            {
                Id = currentUser.Id,
                UserName = currentUser.UserName,
                FirstName = currentUser.Firstname,
                LastName = currentUser.Lastname,
                Street = currentUser.Street,
                PhoneNumber = currentUser.PhoneNumber,
                Zipcode = currentUser.Zipcode,
                City = currentUser.City,
                Country = currentUser.Country,
                BankNumber = currentUser.BankNumber,
                ProfileImageUrl = currentUser.ProfileImageUrl
            };

            //de jsonobject met nieuwe profiel wordt opgehaald
            var x = JsonConvert.DeserializeObject<ApplicationUser>(jsonProfile);

            //de jsonobject met nieuwe profiel wordt omgezet naar de juiste model
            var tempProfile = new ProfileViewModel {
            Id = userid,
            UserName = currentUser.UserName,
            FirstName = currentUser.Firstname,
            LastName = currentUser.Lastname,
            Street = x.Street,
            PhoneNumber = x.PhoneNumber,
            Zipcode = x.Zipcode,
            City = x.City,
            Country = x.Country,
            BankNumber = x.BankNumber,
            ProfileImageUrl = x.ProfileImageUrl
            };

            var profiles = new ConfirmProfileViewModel();
            
            //huidige profiel is currentProfile
            //nieuwe profiel is tempProfile
            //deze profielen worden samengevoegd in een model om door te geven aan de view
            profiles.Profiles.Add(tempProfile);         //index 0
            profiles.Profiles.Add(currentProfile);      //index 1

            return View(@"~/Views/Account/Profile/ConfirmProfile.cshtml", profiles);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AcceptedProfile(ConfirmProfileViewModel model)
        {
            //deze actie kan alleen de admin uitvoeren
            //ingelogde user moet een admin zijn
            //huidige admin user object wordt opgehaald
            var adminid = userManager.GetUserId(HttpContext.User);
            var adminuser = await userManager.FindByIdAsync(adminid);

            //user van te beoordelen profiel wordt uit de model gehaald middels userid
            var userid = model.Profiles[0].Id;
            ApplicationUser currentUser = await userManager.FindByIdAsync(userid);

            if (currentUser == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userid} cannot be found";
                return View("NotFound");
            }
            //nieuwe profiel wordt erin gezet door tempprofile over te nemen naar currentprofile
            else
            {
                currentUser.Street = model.Profiles[0].Street;
                currentUser.PhoneNumber = model.Profiles[0].PhoneNumber;
                currentUser.Zipcode = model.Profiles[0].Zipcode;
                currentUser.City = model.Profiles[0].City;
                currentUser.Country = model.Profiles[0].Country;
                currentUser.BankNumber = model.Profiles[0].BankNumber;
                currentUser.ProfileImageUrl = model.Profiles[0].ProfileImageUrl;
                currentUser.NewProfile = null;

                // Update the user using UpdateAsync
                var result = await userManager.UpdateAsync(currentUser);

                if (result.Succeeded)
                {
                    var activeUser = userManager.FindByIdAsync(userManager.GetUserId(HttpContext.User)).Result;
                // activiteit wordt gelogd
                    repox.LogActivity(activeUser, "AcceptedProfile", $"{activeUser.Firstname[0]}. {activeUser.Lastname} heeft profielverzoek van {currentUser.Firstname[0]}. {currentUser.Lastname} goedgekeurd.");
                // mail wordt naar gebruiker verstuurd met bericht goedkeuring
                    mailServer.SendAcceptedProfileMail(currentUser.UserName, currentUser.Firstname, currentUser.Lastname, adminuser.Firstname, adminuser.Lastname);
                    return View(@"~/Views/Account/Profile/StatusProfile.cshtml");
                }
                return View(currentUser);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeniedProfile(ConfirmProfileViewModel model)
        {
            //deze actie kan alleen de admin uitvoeren
            //ingelogde user moet een admin zijn
            //huidige admin user object wordt opgehaald
            var adminid = userManager.GetUserId(HttpContext.User);
            var adminuser = await userManager.FindByIdAsync(adminid);

            //user van te beoordelen profiel wordt uit de model gehaald middels userid
            var userid = model.Profiles[0].Id;
            ApplicationUser currentUser = await userManager.FindByIdAsync(userid);

            if (currentUser == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userid} cannot be found";
                return View("NotFound");
            }
            else
            {
                //de wijzigen in het profiel dat werd opgeslagen in de newprofile property wordt leeg gemaakt. omdat het profiel is afgekeurd
                currentUser.NewProfile = null;
                // Update the user using UpdateAsync
                var result = await userManager.UpdateAsync(currentUser);

                if (result.Succeeded)
                {
                    var activeUser = userManager.FindByIdAsync(userManager.GetUserId(HttpContext.User)).Result;
                //activiteit wordt gelogd
                    repox.LogActivity(activeUser, "DeniedProfile", $"{activeUser.Firstname[0]}. {activeUser.Lastname} heeft profielverzoek van {currentUser.Firstname[0]}. {currentUser.Lastname} afgewezen.");
                //er wordt een mail verzonden aan de gebruiker, dat profiel is afgekeurd
                    mailServer.SendDeclinedProfileMail(currentUser.UserName, currentUser.Firstname, currentUser.Lastname, adminuser.Firstname, adminuser.Lastname);

                    return View(@"~/Views/Account/Profile/DeniedProfile.cshtml");
                }
                return View(currentUser);
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Overzicht()
        {
            //var lijst = await userManager.Users.Where(u => u.Country == "Nederland" ).ToListAsync();
            var lijst = await userManager.Users.Where(u => u.NewProfile != null ).ToListAsync();

            return View(@"~/Views/Account/Profile/Overzicht.cshtml", lijst);
        }
    }
}