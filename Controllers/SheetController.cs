﻿using System;
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
using QienUrenMachien.Hubs;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;

namespace QienUrenMachien.Controllers
{
    public class SheetController : Controller
    {
        private readonly ITimeSheetRepository repo;

        //public IActionResult Month();

        private readonly UserManager<ApplicationUser> userManager;
        private readonly IActivityLogRepository repox;
        private readonly IHubContext<ChatHub> hub;
        private readonly MailServer mailServer;
        private readonly DataController dataController;

        public SheetController(ITimeSheetRepository repo, UserManager<ApplicationUser> userManager, IActivityLogRepository repox, IHubContext<ChatHub> hub)
        {
            this.repo = repo;
            this.userManager = userManager;
            this.repox = repox;
            this.hub = hub;
            this.mailServer = new MailServer();
        }

        //public IActionResult Index()
        //{
        //    return View("Month");
        //}

        [Route("Sheet/ConfirmTimeSheet/{url}")]
        [HttpGet]
        public async Task<IActionResult> ConfirmTimeSheet(string url)
        {
            ViewBag.url = url;

            if (url == null)
            {
                ViewBag.ErrorMessage = $"Sheet with Id = {url} cannot be found";
                return View("NotFound");
            };

            var result = await repo.GetOneTimeSheetByUrl(url);
            return View(result);
        }

        [Route("Sheet/RejectedTimeSheet/{url}")]
        [HttpGet]
        public async Task<IActionResult> RejectedTimeSheet(string url)
        {
            ViewBag.url = url;

            if (url == null)
            {
                ViewBag.ErrorMessage = $"Sheet with Id = {url} cannot be found";
                return View("NotFound");
            };

            var result = await repo.GetOneTimeSheetByUrl(url);
            return View(result);
        }

        [Route("Sheet/ApproveTimeSheet/{id}")]
        [HttpGet]
        public async Task<IActionResult> ApproveTimeSheet(int id)
        {
            var _timeSheet = await repo.GetTimeSheet(id);

            _timeSheet.Approved = "goedgekeurd";
            var result = await repo.UpdateTimeSheet(_timeSheet);

            ApplicationUser user = await userManager.FindByIdAsync(_timeSheet.Id);

            var activeUser = userManager.FindByIdAsync(userManager.GetUserId(HttpContext.User)).Result;
            repox.LogActivity(activeUser, "ApproveTimeSheet", $"{activeUser.Firstname[0]}. {activeUser.Lastname} heeft urenformulier {_timeSheet.Month} van {user.Firstname[0]}. {user.Lastname} goedgekeurd.");

            mailServer.SendApprovalMail(user.UserName, user.Firstname + " " + user.Lastname, _timeSheet.Month, "approved");

            return RedirectToAction("confirmtimesheet", "sheet", new { url = _timeSheet.Url });
        }

        [Route("Sheet/RejectTimeSheet/{id}")]
        [HttpGet]
        public async Task<IActionResult> RejectTimeSheet(int id)
        {
            var _timeSheet = await repo.GetTimeSheet(id);

            _timeSheet.Approved = "afgewezen";
            var result = await repo.UpdateTimeSheet(_timeSheet);

            ApplicationUser user = await userManager.FindByIdAsync(_timeSheet.Id);


            var activeUser = userManager.FindByIdAsync(userManager.GetUserId(HttpContext.User)).Result;
            repox.LogActivity(activeUser, "RejectTimeSheet", $"{activeUser.Firstname[0]}. {activeUser.Lastname} heeft urenformulier {_timeSheet.Month} van {user.Firstname[0]}. {user.Lastname} afgewezen.");

            mailServer.SendApprovalMail(user.UserName, user.Firstname + " " + user.Lastname, _timeSheet.Month,"rejected");

            var admins = await userManager.GetUsersInRoleAsync("Admin");

            foreach (ApplicationUser admin in admins)
            {
                mailServer.AdminRejectedTimeSheet(admin.UserName, _timeSheet.Url, user.Firstname + " " + user.Lastname, admin.Firstname + " " + admin.Lastname, _timeSheet.Month);
            }

            return RedirectToAction("confirmtimesheet", "sheet", new { url = _timeSheet.Url });
        }

        [Route("Sheet/OpenTimeSheet/{url}")]
        [HttpGet]
        public async Task<IActionResult> OpenTimeSheet(string url)
        {
            var _timeSheet = repo.GetOneTimeSheet(url);

            _timeSheet.Approved = "Not submitted";
            _timeSheet.Submitted = false;
            var result = await repo.UpdateTimeSheet(_timeSheet);

            ApplicationUser user = await userManager.FindByIdAsync(_timeSheet.Id);


            var activeUser = userManager.FindByIdAsync(userManager.GetUserId(HttpContext.User)).Result;
            repox.LogActivity(activeUser, "OpenTimeSheet", $"{activeUser.Firstname[0]}. {activeUser.Lastname} heeft urenformulier {_timeSheet.Month} van {user.Firstname[0]}. {user.Lastname} geopend.");

            mailServer.OpenTimeSheetMail(user.UserName, user.Firstname + " " + user.Lastname , _timeSheet.Month, _timeSheet.Url);

            var admins = await userManager.GetUsersInRoleAsync("Admin");

            return RedirectToAction("rejectedtimesheet", "sheet", new { url = _timeSheet.Url });
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

        [Route("Sheet/Overview")]
        [HttpGet]
        public async Task<IActionResult> Overview()
        {
            
            string id = userManager.GetUserId(User);
            ApplicationUser user = await userManager.FindByIdAsync(id);
            var result = await repo.GetUserOverview(id, user.ActiveFrom, user.ActiveUntil);
            //repo.AddTimeSheetTemp();

            return View(result);

        }


        [Route("Sheet/Overview/{url}")]
        [HttpGet]
        public async Task<IActionResult> UserTimeSheet(string url)
        {
            var result = await repo.GetOneTimeSheetByUrl(url);
            return View(result);

        }

        [HttpGet]
        public async Task<IActionResult> AdminViewSheet(string url)
        {
            var result = await repo.GetOneTimeSheetByUrl(url);

            return View(result);

        }

        [Route("Sheet/SubmitTimeSheet#{url}")]
        [HttpGet]
        public async Task<IActionResult> SubmitTimeSheet(string url)
        {
            var _timeSheet = await repo.GetTimeSheetUrl(url);
            var currentWerknemer = await userManager.FindByIdAsync(_timeSheet.Id);
            var currentWerkgever = await userManager.FindByIdAsync(currentWerknemer.WerkgeverID);
            _timeSheet.Submitted = true;
            var result = await repo.UpdateTimeSheet(_timeSheet);

            var activeUser = userManager.FindByIdAsync(userManager.GetUserId(HttpContext.User)).Result;
            repox.LogActivity(activeUser, "SubmitTimeSheet", $"{activeUser.Firstname[0]}. {activeUser.Lastname} heeft urenformulier {_timeSheet.Month} ingediend.");

            //call hub
            var lastActivity = repox.GetLastLog();
            await hub.Clients.All.SendAsync("ReceiveActivity", lastActivity);


            mailServer.SendConfirmationMail(currentWerkgever.UserName, currentWerkgever.Firstname, "https://localhost:44398/sheet/confirmtimesheet/" + result.Url, (currentWerknemer.Firstname + " " + currentWerknemer.Lastname), _timeSheet.Month);

            //return RedirectToAction("usertimesheet", "sheet", new { url = _timeSheet.Url });
            return RedirectToAction("SheetAttachment", "upload", new { SheetID = _timeSheet.SheetID });
        }

        [Route("Sheet/UnSubmitTimeSheet/{url}")]
        [HttpGet]
        public async Task<IActionResult> UnSubmitTimeSheet(string url)
        {
            var _timeSheet = await repo.GetTimeSheetUrl(url);

            _timeSheet.Submitted = false;
            var result = await repo.UpdateTimeSheet(_timeSheet);

            var activeUser = userManager.FindByIdAsync(userManager.GetUserId(HttpContext.User)).Result;
            repox.LogActivity(activeUser, "UnSubmitTimeSheet", $"{activeUser.Firstname[0]}. {activeUser.Lastname} heeft urenformulier {_timeSheet.Month} ingetrokken.");


            //call hub
            var lastActivity = repox.GetLastLog();
            await hub.Clients.All.SendAsync("ReceiveActivity", lastActivity);

            return RedirectToAction("usertimesheet", "sheet", new { url = _timeSheet.Url });
        }

        public async Task<FileContentResult> DownloadCSV(string url)
        {
            var _timeSheet = await repo.GetTimeSheetUrl(url);
            ApplicationUser user = await userManager.FindByIdAsync(_timeSheet.Id);

            var data = JObject.Parse(_timeSheet.Data);

            string csv = "Dag,Project,Overwerk,Ziek,Afwezig,Training,Overig\n";

            foreach(KeyValuePair<string, JToken> entry in data)
            {
                Console.WriteLine(entry.Value);
                var ProjectHours = entry.Value["projectHours"];
                var Overwork = entry.Value["overwork"];
                var Sick = entry.Value["sick"];
                var Absence = entry.Value["absence"];
                var Training = entry.Value["training"];
                var Other = entry.Value["other"];

                csv += $"{entry.Key},{ProjectHours},{Overwork},{Sick},{Absence},{Training},{Other}\n";
            }
            string date = DateTime.Now.ToString("yyyyMMddTHHmmss");
            return File(new System.Text.UTF8Encoding().GetBytes(csv), "txt/csv", $"timesheet_{user.Firstname}_{user.Lastname}_{date}.csv");
        }

    }
}
