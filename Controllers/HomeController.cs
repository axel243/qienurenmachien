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
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<HomeController> _logger;
        private readonly IActivityLogRepository repox;

        public HomeController(SignInManager<ApplicationUser> signInManager, ILogger<HomeController> logger, IActivityLogRepository repox)
        {
            this.signInManager = signInManager;
            _logger = logger;
            this.repox = repox;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            var logs = repox.GetActivityLogs();
            logs.Reverse();

            var model = new DashboardViewModel();
            model.activityLogViewModels = logs;

        if (signInManager.IsSignedIn(User) && (User.IsInRole("Werknemer") || User.IsInRole("Trainee")))
            {
                return RedirectToAction("overview", "sheet");
            }
            return View(model);
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
