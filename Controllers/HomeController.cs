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


        private readonly ITimeSheetRepository repo;


        private readonly UserManager<ApplicationUser> userManager;



        public HomeController(SignInManager<ApplicationUser> signInManager, ILogger<HomeController> logger, IActivityLogRepository repox, UserManager<ApplicationUser> userManager , ITimeSheetRepository repo)
        {
            this.signInManager = signInManager;
            _logger = logger;
            this.userManager = userManager;
            this.repo = repo;
            this.repox = repox;
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
                
                var logs = repox.GetActivityLogs();
                logs.Reverse();

                var model = new DashboardViewModel();
                model.timeSheetWithUsers = new List<TimeSheetWithUser>();
                model.activityLogViewModels = logs;
                var users = await repo.GetTimeSheetAndUser();
                var profileEdits = await repo.GetLastMonthProfileEdits();
                foreach (var user in users)
                {
                    var newTimeSheet = new TimeSheetWithUser();
                    newTimeSheet.FirstName = user.FirstName;
                    newTimeSheet.LastName = user.LastName;
                    if(user.Status =="Not Submitted" || user.Status == "Not submitted")
                    {
                        newTimeSheet.Status = "Niet ingeleverd";
                    }
                    else
                    {
                        newTimeSheet.Status = "Afgewezen timesheet";
                    }
                    
                    newTimeSheet.url = user.url;
                    //newTimeSheet.WerkgeverId = user.WerkgeverId;

                    model.timeSheetWithUsers.Add(newTimeSheet);
                }
                foreach(var profile in profileEdits)
                {
                    var newProfile = new TimeSheetWithUser();
                        newProfile.FirstName = profile.FirstName;
                        newProfile.LastName = profile.LastName;
                        newProfile.Status = "Heeft een verzoek van profielwijziging ingediend";
                        newProfile.url = profile.userId;
                        model.timeSheetWithUsers.Add(newProfile);                                  
                }

                return View(model);
            }
            else
            {
                //if (signInManager.IsSignedIn(User) && User.IsInRole("Werknemer"))
                //{
                //    return RedirectToAction("overview", "sheet");
                //}
                return RedirectToAction("overview", "sheet");
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
        public async Task<FileContentResult> DownloadCSV()
        {
            string csv = await repo.TimeSheetDataCSV();
            return File(new System.Text.UTF8Encoding().GetBytes(csv), "text/csv", "Report123.csv");
        }
    }
}
