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
            var userid = userManager.GetUserId(HttpContext.User);

            if (userid == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                var user = userManager.FindByIdAsync(userid).Result;
                return View(@"~/Views/Account/Profile/Profile.cshtml", user);
            }
        }


        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var userid = userManager.GetUserId(HttpContext.User);

            var currentUser = await userManager.FindByIdAsync(userid);

            if (currentUser == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userid} cannot be found";
                return View("NotFound");
            }

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
                var userid = model.Id;
            ApplicationUser currentUser = await userManager.FindByIdAsync(userid);

            var adminlist = await userManager.GetUsersInRoleAsync("Admin");

            if (currentUser == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userid} cannot be found";
                return View("NotFound");
            } else if (adminlist.Contains(currentUser))
            {
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
            else
            {
                if (model.ProfileImage != null)
                {
                    UploadImage();
                }

                var jsonProfile = JsonConvert.SerializeObject(model);

                currentUser.NewProfile = jsonProfile;

                // Update the user using UpdateAsync
                var result = await userManager.UpdateAsync(currentUser);

                if (result.Succeeded)
                {
                    var activeUser = userManager.FindByIdAsync(userManager.GetUserId(HttpContext.User)).Result;
                    repox.LogActivity(activeUser, "EditProfile", $"{activeUser.UserName} heeft profiel verzoek ingediend.");

                    //call hub
                    var lastActivity = repox.GetLastLog();
                    await hub.Clients.All.SendAsync("ReceiveActivity", lastActivity);

                    mailServer.SendEditedProfileMail(currentUser.UserName, currentUser.Firstname);
                    return View(@"~/Views/Account/Profile/StatusProfile.cshtml");
                }

                //methode om profiel foto te uploaden
                void UploadImage()
                {
                    var dir = env.ContentRootPath;
                    var file = model.ProfileImage;
                    var uploadPath = dir + $@"\Uploads\Images\{currentUser.UserName}";

                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }


                    using (var fileStream = new FileStream(Path.Combine(uploadPath, model.ProfileImage.FileName), FileMode.Create, FileAccess.Write))
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
            // in parameter string id
            //var userid = id;
            var userid = id;

            var currentUser = await userManager.FindByIdAsync(userid);

            if (currentUser == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userid} cannot be found";
                return View(@"~/Views/Account/NotFound.cshtml");
            }

            var jsonProfile = currentUser.NewProfile;

            
            var currentProfile = new ProfileViewModel
            {
                Id = currentUser.Id,
                Street = currentUser.Street,
                PhoneNumber = currentUser.PhoneNumber,
                Zipcode = currentUser.Zipcode,
                City = currentUser.City,
                Country = currentUser.Country,
                BankNumber = currentUser.BankNumber,
                ProfileImageUrl = currentUser.ProfileImageUrl
            };

            var x = JsonConvert.DeserializeObject<ApplicationUser>(jsonProfile);
            var tempProfile = new ProfileViewModel {
            Id = userid,
            Street = x.Street,
            PhoneNumber = x.PhoneNumber,
            Zipcode = x.Zipcode,
            City = x.City,
            Country = x.Country,
            BankNumber = x.BankNumber,
            ProfileImageUrl = x.ProfileImageUrl
            };

            var profiles = new ConfirmProfileViewModel();
            
            profiles.Profiles.Add(tempProfile);         //index 0
            profiles.Profiles.Add(currentProfile);      //index 1
            
            


            return View(@"~/Views/Account/Profile/ConfirmProfile.cshtml", profiles);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AcceptedProfile(ConfirmProfileViewModel model)
        {
            var adminid = userManager.GetUserId(HttpContext.User);
            var adminuser = await userManager.FindByIdAsync(adminid);
            var userid = model.Profiles[0].Id;
            ApplicationUser currentUser = await userManager.FindByIdAsync(userid);

            if (currentUser == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userid} cannot be found";
                return View("NotFound");
            }
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
                    repox.LogActivity(activeUser, "AcceptedProfile", $"{activeUser.UserName} heeft profielverzoek van {currentUser.UserName} goedgekeurd.");
                    mailServer.SendAcceptedProfileMail(currentUser.UserName, adminuser.UserName);
                    return View(@"~/Views/Account/Profile/StatusProfile.cshtml");
                }
                return View(currentUser);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeniedProfile(ConfirmProfileViewModel model)
        {
            var adminid = userManager.GetUserId(HttpContext.User);
            var adminuser = await userManager.FindByIdAsync(adminid);

            var userid = model.Profiles[0].Id;
            ApplicationUser currentUser = await userManager.FindByIdAsync(userid);

            if (currentUser == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userid} cannot be found";
                return View("NotFound");
            }
            else
            {
                // field in db legen
                currentUser.NewProfile = null;
                // Update the user using UpdateAsync
                var result = await userManager.UpdateAsync(currentUser);

                if (result.Succeeded)
                {
                    var activeUser = userManager.FindByIdAsync(userManager.GetUserId(HttpContext.User)).Result;
                    repox.LogActivity(activeUser, "DeniedProfile", $"{activeUser.UserName} heeft profielverzoek van {currentUser.UserName} afgewezen.");

                    mailServer.SendDeclinedProfileMail(currentUser.UserName, adminuser.UserName);

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