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

namespace QienUrenMachien.Controllers
{
    public class MailController : Controller

    {
        private readonly ITimeSheetRepository repo;

        public MailController(ITimeSheetRepository repo)
        {
            this.repo = repo;
        }


        // GET: /<controller>/
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> TestMail()
        {
            TimeSheet _timeSheet = await repo.GetTimeSheet(1);

            _timeSheet.Url = Guid.NewGuid().ToString();
            var result = await repo.UpdateTimeSheet(_timeSheet);

            if (result != null)
            {
                using (var message = new MailMessage())
                {
                    message.To.Add(new MailAddress("j.m.r.kramer@gmail.com", "To Name"));
                    message.From = new MailAddress("info@qienurenmachien.nl", "Qien Uren Machien");
                    message.CC.Add(new MailAddress("cc@email.com", "CC Name"));
                    message.Bcc.Add(new MailAddress("bcc@email.com", "BCC Name"));
                    message.Subject = "Lever je timesheet in lul";
                    message.Body = "https://localhost:44398/sheet/confirmtimesheet/" + result.Url;
                    message.IsBodyHtml = true;

                    using (var client = new SmtpClient("smtp.gmail.com"))
                    {
                        client.Port = 587;
                        client.Credentials = new NetworkCredential("qienurenmachien@gmail.com", "Test1234!");
                        client.EnableSsl = true;
                        client.Send(message);
                    }



                }
            }

            return View();

        }
    }
}