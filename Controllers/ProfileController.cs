using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using QienUrenMachien.Entities;
using QienUrenMachien.Mail;
using QienUrenMachien.Repositories;

namespace QienUrenMachien.Controllers
{
    public class ProfileController : Controller
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IActivityLogRepository repox;
        private readonly MailServer mailServer;

        public ProfileController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IActivityLogRepository repox)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.repox = repox;
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
            return View(@"~/Views/Account/Profile/EditProfile.cshtml", currentUser);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(ApplicationUser model)
        {
                var userid = model.Id;
            ApplicationUser currentUser = await userManager.FindByIdAsync(userid);

            if (currentUser == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                var jsonProfile = JsonConvert.SerializeObject(model);

                currentUser.NewProfile = jsonProfile;

                // Update the user using UpdateAsync
                var result = await userManager.UpdateAsync(currentUser);

                if (result.Succeeded)
                {
                    var activeUser = userManager.FindByIdAsync(userManager.GetUserId(HttpContext.User)).Result;
                    repox.LogActivity(activeUser, "EditProfile", $"{activeUser.UserName} heeft profiel verzoek ingediend.");

                    mailServer.SendEditedProfileMail(currentUser.UserName, currentUser.Firstname);
                    return View(@"~/Views/Account/Profile/StatusProfile.cshtml");
                }
                return View(currentUser);
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

            ViewBag.User = currentUser;

            var x = JsonConvert.DeserializeObject<ApplicationUser>(jsonProfile);
            ApplicationUser tempUser = new ApplicationUser();
            tempUser.Id = userid;
            tempUser.Street = x.Street;
            tempUser.PhoneNumber = x.PhoneNumber;
            tempUser.Zipcode = x.Zipcode;
            tempUser.City = x.City;
            tempUser.Country = x.Country;
            tempUser.BankNumber = x.BankNumber;
            tempUser.ProfileImageUrl = x.ProfileImageUrl;

            return View(@"~/Views/Account/Profile/ConfirmProfile.cshtml", tempUser);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AcceptedProfile(ApplicationUser model)
        {
            var adminid = userManager.GetUserId(HttpContext.User);
            var adminuser = await userManager.FindByIdAsync(adminid);
            var userid = model.Id;
            ApplicationUser currentUser = await userManager.FindByIdAsync(userid);

            if (currentUser == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userid} cannot be found";
                return View("NotFound");
            }
            else
            {

                currentUser.Street = model.Street;
                currentUser.PhoneNumber = model.PhoneNumber;
                currentUser.Zipcode = model.Zipcode;
                currentUser.City = model.City;
                currentUser.Country = model.Country;
                currentUser.BankNumber = model.BankNumber;
                currentUser.ProfileImageUrl = model.ProfileImageUrl;
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
        public async Task<IActionResult> DeniedProfile(ApplicationUser model)
        {
            var adminid = userManager.GetUserId(HttpContext.User);
            var adminuser = await userManager.FindByIdAsync(adminid);

            var userid = model.Id;
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