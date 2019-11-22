using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QienUrenMachien.Models;
using QienUrenMachien.Repositories;
using Microsoft.AspNetCore.Identity;
using System.Net.Mail;
using System.Net;
using QienUrenMachien.Entities;
using QienUrenMachien.Mail;

namespace QienUrenMachien.Controllers
{
    public class SheetController : Controller
    {
        private readonly ITimeSheetRepository repo;
        private readonly UserManager<ApplicationUser> userManager;

        private readonly MailServer mailServer;

        public SheetController(ITimeSheetRepository repo, UserManager<ApplicationUser> userManager)
        {
            this.repo = repo;
            this.userManager = userManager;
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


        public IActionResult TimeSheet(int Year, int Month)
        {
            var result = GetAllDaysInMonth(Year, Month);
            return View(result);
        }

        public List<DateTime> GetAllDaysInMonth(int year, int month)
        {
            var ret = new List<DateTime>();
            for (int i = 1; i <= DateTime.DaysInMonth(year, month); i++)
            {
                ret.Add(new DateTime(year, month, i));
            }
            return ret;
        }
    }
}
