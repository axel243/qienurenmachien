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

namespace QienUrenMachien.Mail
{
    public class MailServer
    {
        private void SendMail(string recipient, string subject, string body)
        {
            using (var message = new MailMessage())
            {
                message.To.Add(new MailAddress( recipient, "To Name"));
                message.From = new MailAddress("info@qienurenmachien.nl", "Qien Uren Machien");
                message.CC.Add(new MailAddress("cc@email.com", "CC Name"));
                message.Bcc.Add(new MailAddress("bcc@email.com", "BCC Name"));
                message.Subject = subject;
                message.Body = body;
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

        public void SendConfirmationMail(string recipient, string body)
        {
            string subject = "Nieuwe timesheet ingediend.";

            SendMail(recipient, subject, body);
        }

        public void SendApprovalMail(string recipient, string body)
        {
            string subject = $"Timesheet {body}";

            SendMail(recipient, subject, body);
        }
        
           
    }
}