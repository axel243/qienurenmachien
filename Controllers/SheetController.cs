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

namespace QienUrenMachien.Controllers
{
    public class SheetController : Controller
    {
        private readonly ITimeSheetRepository repo;
        private readonly UserManager<ApplicationUser> userManager;

        public SheetController(ITimeSheetRepository repo, UserManager<ApplicationUser> userManager)
        {
            this.repo = repo;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View("Month");
        }

        // public IActionResult ConfirmTimeSheet(string url)
        // {
        //     Console.WriteLine("##################");
        //     Console.WriteLine(url);
        //     Console.WriteLine(url);
        //     Console.WriteLine("##################");
        //     var result = repo.GetOneTimeSheet("4db457e1-1b69-4da0-8d02-0979749471ba");
        //     return View(result);
        // }

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

            using (var message = new MailMessage())
            {
                message.To.Add(new MailAddress(user.UserName, "To Name"));
                message.From = new MailAddress("info@qienurenmachien.nl", "Qien Uren Machien");
                message.CC.Add(new MailAddress("cc@email.com", "CC Name"));
                message.Bcc.Add(new MailAddress("bcc@email.com", "BCC Name"));
                message.Subject = "Je uren zijn goedgekeurd";
                message.Body = "money money money!!!!!";
                message.IsBodyHtml = true;

                using (var client = new SmtpClient("smtp.gmail.com"))
                {
                    client.Port = 587;
                    client.Credentials = new NetworkCredential("qienurenmachien@gmail.com", "Test1234!");
                    client.EnableSsl = true;
                    client.Send(message);
                }



            }

            return RedirectToAction("confirmtimesheet", "sheet", new { url = _timeSheet.Url });
        }

        [Route("Sheet/RejectTimeSheet/{id}")]
        [HttpGet]
        public async Task<IActionResult> RejectTimeSheet(int id)
        {
            var _timeSheet = await repo.GetTimeSheet(id);

            _timeSheet.Approved = "Rejected";
            var result = await repo.UpdateTimeSheet(_timeSheet);
            Console.WriteLine("######");

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
