using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QienUrenMachien.Data;
using QienUrenMachien.Models;
using QienUrenMachien.Repositories;
using Microsoft.AspNetCore.Identity;
using System.Net.Mail;
using System.Net;
using QienUrenMachien.Entities;
using QienUrenMachien.Mail;
using System.Text.Json;

namespace QienUrenMachien.Controllers
{
    public class SheetController : Controller
    {
        private readonly ITimeSheetRepository repo;

        //public IActionResult Month();

        private readonly UserManager<ApplicationUser> userManager;
        private readonly IActivityLogRepository repox;
        private readonly MailServer mailServer;

        public SheetController(ITimeSheetRepository repo, UserManager<ApplicationUser> userManager, IActivityLogRepository repox)
        {
            this.repo = repo;
            this.userManager = userManager;
            this.repox = repox;
            this.mailServer = new MailServer();
        }

        public IActionResult Index()
        {
            return View("Month");
        }

        [Route("Sheet/ConfirmTimeSheet/{url}")]
        [HttpGet]
        public IActionResult ConfirmTimeSheet(string url)
        {
            ViewBag.url = url;

            if (url == null)
            {
                ViewBag.ErrorMessage = $"Sheet with Id = {url} cannot be found";
                return View("NotFound");
            };

            var result = repo.GetOneTimeSheet(url);
            return View(result);
        }

        [Route("Sheet/ApproveTimeSheet/{id}")]
        [HttpGet]
        public async Task<IActionResult> ApproveTimeSheet(int id)
        {
            var _timeSheet = await repo.GetTimeSheet(id);

            _timeSheet.Approved = "Approved";
            var result = await repo.UpdateTimeSheet(_timeSheet);

            ApplicationUser user = await userManager.FindByIdAsync(_timeSheet.Id);

            mailServer.SendApprovalMail(user.UserName, "Approved");

            return RedirectToAction("confirmtimesheet", "sheet", new { url = _timeSheet.Url });
        }

        [Route("Sheet/RejectTimeSheet/{id}")]
        [HttpGet]
        public async Task<IActionResult> RejectTimeSheet(int id)
        {
            var _timeSheet = await repo.GetTimeSheet(id);

            _timeSheet.Approved = "Rejected";
            var result = await repo.UpdateTimeSheet(_timeSheet);

            ApplicationUser user = await userManager.FindByIdAsync(_timeSheet.Id);

            mailServer.SendApprovalMail(user.UserName, "Rejected");

            return RedirectToAction("confirmtimesheet", "sheet", new { url = _timeSheet.Url });
        }

        //public IActionResult TimeSheet(int Year, int Month)
        //{
        //    timesheet.Year = Year;
        //    timesheet.Month = Month;
        //    var result = SetAllDaysInMonth();
        //    return View(result);
        //}

        //public TimeSheet SetAllDaysInMonth()
        //{
        //    List<Day> days = new List<Day>();
        //    for (int i = 1; i <= DateTime.DaysInMonth(timesheet.Year, timesheet.Month); i++)
        //    {
        //        days.Add(new Day(new DateTime(timesheet.Year, timesheet.Month, i), null, 0, 0, 0, 0, 0, 0, null));
        //    }
        //    timesheet.days = days;
        //    return timesheet;

        //}

        //public string DaysToData()
        //{
        //    var jsonString = JsonConvert.SerializeObject(timesheet.days);
        //    return jsonString;
        //}


        //[HttpPost]
        //public ActionResult AddSheet(TimeSheet timesheet)
        //{
        //    this.timesheet = timesheet;
        //    if (!ModelState.IsValid)
        //        return View(timesheet);
        //    timesheet.Data = DaysToData();
        //    timesheet.ProjectHours = timesheet.days.Sum(d => d.ProjectHours);
        //    timesheet.Overwork = timesheet.days.Sum(d => d.Overwork);
        //    timesheet.Sick = timesheet.days.Sum(d => d.Sick);
        //    timesheet.Absence = timesheet.days.Sum(d => d.Absence);
        //    timesheet.Training = timesheet.days.Sum(d => d.Training);
        //    timesheet.Other = timesheet.days.Sum(d => d.Other);
        //    timesheet.Submitted = 1;
        //    repo.Add(timesheet);
        //    return RedirectToAction("Month");
        //}

        //[Route("Sheet/UserTimeSheet/")]
        //[HttpGet]
        //public async Task<IActionResult> UserTimeSheet()
        //{
        //    while (true){
        //        try {
        //            var result = await repo.GetOneTimeSheetAsync(userManager.GetUserId(User), "January");
        //            return View(result);
        //        }
                
        //        catch {
        //            int nDays = DateTime.DaysInMonth(2019, 1);
        //            string data = "{";

        //        for (int i = 1; i <= nDays; i++)
        //        {
        //            DayJulian _day = new DayJulian();
        //            data += $"\"{i}\": " + JsonSerializer.Serialize<DayJulian>(_day);
        //            if (i != nDays)
        //            {
        //                data += ", ";
        //            }
        //        }
        //        data += "}";

        //        TimeSheet entity2 = repo.AddTimeSheet(userManager.GetUserId(User), data);
        //        }
        //    }

        //}

        [Route("Sheet/Overview/")]
        [HttpGet]
        public async Task<IActionResult> Overview()
        {

            var result = await repo.GetUserOverview(userManager.GetUserId(User));

            return View(result);

        }


        [Route("Sheet/Overview/{url}")]
        [HttpGet]
        public async Task<IActionResult> UserTimeSheet(string url)
        {
            var result = await repo.GetOneTimeSheetByUrl(url);

            return View(result);

        }

        [Route("Sheet/SubmitTimeSheet/{url}")]
        [HttpGet]
        public async Task<IActionResult> SubmitTimeSheet(string url)
        {
            var _timeSheet = await repo.GetTimeSheetUrl(url);
            var currentWerknemer = await userManager.FindByIdAsync(_timeSheet.Id);
            var currentWerkgever = await userManager.FindByIdAsync(currentWerknemer.WerkgeverID);
            _timeSheet.Submitted = true;
            var result = await repo.UpdateTimeSheet(_timeSheet);
            repox.LogActivity(userManager.FindByIdAsync(userManager.GetUserId(HttpContext.User)).Result, "SubmitTimeSheet", "Submitted");
            mailServer.SendConfirmationMail(currentWerkgever.UserName, "https://localhost:44398/sheet/confirmtimesheet/" + result.Url, (currentWerknemer.Firstname + " " + currentWerknemer.Lastname) );

            return RedirectToAction("usertimesheet", "sheet", new { url = _timeSheet.Url });
        }

        [Route("Sheet/UnSubmitTimeSheet/{url}")]
        [HttpGet]
        public async Task<IActionResult> UnSubmitTimeSheet(string url)
        {
            var _timeSheet = await repo.GetTimeSheetUrl(url);

            _timeSheet.Submitted = false;
            var result = await repo.UpdateTimeSheet(_timeSheet);

            repox.LogActivity(userManager.FindByIdAsync(userManager.GetUserId(HttpContext.User)).Result, "UnSubmitTimeSheet", "UnSubmitted");

            return RedirectToAction("usertimesheet", "sheet", new { url = _timeSheet.Url });
        }


    }
}
