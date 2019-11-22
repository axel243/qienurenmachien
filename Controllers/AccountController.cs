using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QienUrenMachien.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace QienUrenMachien.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
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
                return View("Profile", user);
            }
        }
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var userid = userManager.GetUserId(HttpContext.User);

            var currentUser = await userManager.FindByIdAsync(userid);

            if (currentUser == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {userid} cannot be found";
                return View("NotFound");
            }
            return View(currentUser);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(ApplicationUser model)
        {
            var userid = model.Id;
            ApplicationUser currentUser = await userManager.FindByIdAsync(userid);

            if (currentUser == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {model.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                currentUser.Street = model.Street;
                currentUser.Zipcode = model.Zipcode;
                currentUser.City = model.City;
                currentUser.Country = model.Country;
                currentUser.ProfileImageUrl = model.ProfileImageUrl;


                // Update the Role using UpdateAsync
                var result = await userManager.UpdateAsync(currentUser);

                if (result.Succeeded)
                {
                    return RedirectToAction("Profile");
                }
                return View(currentUser);
            }
        }




        //[HttpPost]
        //public async Task<ActionResult> EditProfile(ApplicationUser userdetails)
        //{
        //    IdentityResult x = await userManager.UpdateAsync(userdetails);
        //    if (x.Succeeded)
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }
        //    return View(userdetails);
        //}

        //[HttpGet]
        //public IActionResult EditProfile()
        //{

        //    var userid = userManager.GetUserId(HttpContext.User);

        //    if (userid == null)
        //    {
        //        return RedirectToAction("Login", "Account");
        //    }
        //    else
        //    {
        //        ApplicationUser user = userManager.FindByIdAsync(userid).Result;
        //        return View(user);
        //    }
        //}

        // GET: /<controller>/
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {

            return View();
        }

        // GET: /<controller>/
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {

                    return RedirectToAction("index", "home");
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");

            }

            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email};
                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("index", "home");
                }

                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
