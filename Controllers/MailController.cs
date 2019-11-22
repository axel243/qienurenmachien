using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using QienUrenMachien.Repositories;
using QienUrenMachien.Models;
using QienUrenMachien.Entities;
using QienUrenMachien.Mail;
using Microsoft.AspNetCore.Identity;

namespace QienUrenMachien.Controllers
{
    public class MailController : Controller

    {
        private readonly ITimeSheetRepository repo;
        private readonly MailServer mailServer;
        private readonly UserManager<ApplicationUser> userManager;

        public MailController(ITimeSheetRepository repo, UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
            this.repo = repo;
            this.mailServer = new MailServer();
        }


        // GET: /<controller>/
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> TestMail()
        {
            TimeSheet _timeSheet = await repo.GetTimeSheet(userManager.GetUserId(User));

            _timeSheet.Url = Guid.NewGuid().ToString();
            var result = await repo.UpdateTimeSheet(_timeSheet);

            if (result != null)
            {
                mailServer.SendConfirmationMail("j.m.r.kramer@gmail.com", "https://localhost:44398/sheet/confirmtimesheet/" + result.Url);
            }

            return View();

        }
    }
}