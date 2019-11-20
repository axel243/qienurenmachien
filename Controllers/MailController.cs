using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;

namespace QienUrenMachien.Controllers
{
    public class MailController : Controller
    {        // GET: /<controller>/
        [AllowAnonymous]
        [HttpGet]
        public IActionResult TestMail()
        {

            using (var message = new MailMessage())
            {
                message.To.Add(new MailAddress("j.m.r.kramer@gmail.com", "To Name"));
                message.From = new MailAddress("info@qienurenmachien.nl", "Qien Uren Machien");
                message.CC.Add(new MailAddress("cc@email.com", "CC Name"));
                message.Bcc.Add(new MailAddress("bcc@email.com", "BCC Name"));
                message.Subject = "Lever je timesheet in lul";
                message.Body = "Hello World!";
                message.IsBodyHtml = true;

                using (var client = new SmtpClient("smtp.gmail.com"))
                {
                    client.Port = 587;
                    client.Credentials = new NetworkCredential("qienurenmachien@gmail.com", "Test1234!");
                    client.EnableSsl = true;
                    client.Send(message);
                }


                return View();
            }
        }
    }
}