using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QienUrenMachien.Entities;

namespace QienUrenMachien.Controllers
{
    public class ProfileController : Controller
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;

        public ProfileController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
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
                    return View(@"~/Views/Account/Profile/StatusProfile.cshtml");
                }
                return View(currentUser);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmProfile()
        {
            // in parameter string id
            //var userid = id;
            var userid = "a30d8e07-d447-4aa1-84d7-3e0b302b4d01";

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
            tempUser.Street = x.Street;
            tempUser.PhoneNumber = x.PhoneNumber;
            tempUser.Zipcode = x.Zipcode;
            tempUser.City = x.City;
            tempUser.Country = x.Country;
            tempUser.ProfileImageUrl = x.ProfileImageUrl;

            return View(@"~/Views/Account/Profile/ConfirmProfile.cshtml", tempUser);
        }



        [HttpPost]
        public async Task<IActionResult> ConfirmProfile(ApplicationUser model)
        {

            var userid = "a30d8e07-d447-4aa1-84d7-3e0b302b4d01";
            ApplicationUser currentUser = await userManager.FindByIdAsync(userid);

            if (currentUser == null)
            {
                //ViewBag.ErrorMessage = $"User with Id = {model[0].Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                //currentUser.Street = model[0].Street;
                //currentUser.PhoneNumber = model[0].PhoneNumber;
                //currentUser.Zipcode = model[0].Zipcode;
                //currentUser.City = model[0].City;
                //currentUser.Country = model[0].Country;
                //currentUser.ProfileImageUrl = model[0].ProfileImageUrl;
                //currentUser.NewProfile = null;

                currentUser.Street = model.Street;
                currentUser.PhoneNumber = model.PhoneNumber;
                currentUser.Zipcode = model.Zipcode;
                currentUser.City = model.City;
                currentUser.Country = model.Country;
                currentUser.ProfileImageUrl = model.ProfileImageUrl;
                currentUser.NewProfile = null;

                // Update the user using UpdateAsync
                var result = await userManager.UpdateAsync(currentUser);

                if (result.Succeeded)
                {
                    return View(@"~/Views/Account/Profile/StatusProfile.cshtml");
                }
                return View(currentUser);
            }
        }
        //[HttpPost]
        //public async Task<IActionResult> OnPostDeny()
        //{
        //    var userid = "a30d8e07-d447-4aa1-84d7-3e0b302b4d01";
        //    ApplicationUser currentUser = await userManager.FindByIdAsync(userid);

        //    if (currentUser == null)
        //    {
        //        ViewBag.ErrorMessage = $"User with Id = {currentUser.Id} cannot be found";
        //        return View("NotFound");
        //    }
        //    else
        //    {
        //        // field in db legen
        //        currentUser.NewProfile = "";

        //        // Update the user using UpdateAsync
        //        var result = await userManager.UpdateAsync(currentUser);

        //        if (result.Succeeded)
        //        {
        //            return View(@"~/Views/Account/Profile/StatusProfile.cshtml");
        //        }
        //        return View(currentUser);
        //    }
        //}
    }
}