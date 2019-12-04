using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QienUrenMachien.Entities;
using QienUrenMachien.Models;
using QienUrenMachien.Repositories;

namespace QienUrenMachien.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ITimeSheetRepository repo;


        private readonly UserManager<ApplicationUser> userManager;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, ITimeSheetRepository repo)
        {
            _logger = logger;
            this.userManager = userManager;
            this.repo = repo;
        }
        //[AllowAnonymous]
        //public IActionResult Index()
        //{
            
        //    return View();
        //}

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(TimeSheetWithUser timesheets)
        {
            var users = await repo.GetTimeSheetAndUser();
            var table = new List<TimeSheetWithUser>();
            
            foreach (var user in users)
            {
                timesheets.FirstName = user.FirstName;
                timesheets.LastName = user.LastName;
                timesheets.Status = user.Status;
                table.Add(timesheets);
            }
            return View(table);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
