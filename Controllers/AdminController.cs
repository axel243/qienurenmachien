using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QienUrenMachien.Data;

namespace QienUrenMachien.Controllers
{
    public class AdminController : Controller
    {

        private readonly UserManager<IdentityUser> userManager;

        public AdminController(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AdminDashboard()
        {
            var userlist = userManager.Users;
            return View(userlist);
        }

        public IActionResult ViewUser(string Id)
        {
            var singleuser = userManager.Users.Single(u => u.Id == Id);
            return View(singleuser);
        }
    }
}