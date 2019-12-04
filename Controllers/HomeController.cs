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

        [Authorize]
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Admin"))
            {
                var table = new List<TimeSheetWithUser>();
               
                var users = await repo.GetTimeSheetAndUser();
                
                foreach (var user in users)
                {
                    var newTimeSheet = new TimeSheetWithUser();
                    newTimeSheet.FirstName = user.FirstName;
                    newTimeSheet.LastName = user.LastName;
                    newTimeSheet.Status = user.Status;
                    newTimeSheet.url = user.url;
                    //newTimeSheet.WerkgeverId = user.WerkgeverId;

                    table.Add(newTimeSheet);
                }
                
                return View(table);
            }
            else
            {
                return View();
            }
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
