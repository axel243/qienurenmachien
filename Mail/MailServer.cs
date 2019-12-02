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

        public void SendForgotPasswordMail(string recipient, string resetlink)
        {
            string subject = $"Link om het wachtwoord te resetten"; 
            string body = "<br/> <a href=" + resetlink + ">Wachtwoord resetten</a>";
            SendMail(recipient, subject, body);
        }

        public void SendEditedProfileMail(string recipient, string person)
        {
            //recipient zal de admin moeten zijn, nu zijn recipient en person nog gelijk
            string subject = $"Verzoek profiels wijziging van {person}";
            string body = $"{person} heeft een profielswijziging ingediend";
            

            SendMail(recipient, subject, body);
        }
        public void SendAcceptedProfileMail(string recipient, string admin)
        {
            string subject = "Profielwijziging is goedgekeurd";
            string body = $"Het verzoek tot wijziging van je profiel is goedgekeurd door {admin}";

            SendMail(recipient, subject, body);
        }

        public void SendDeclinedProfileMail(string recipient, string admin)
        {
            string subject = "Profielwijziging is afgekeurd";
            string body = $"Het verzoek tot wijziging van je profiel is afgekeurd door {admin}";

            SendMail(recipient, subject, body);
        }

        public void SendRegisterUserMail(string recipient)
        {
            string verificatielink = "https://localhost:44398/Mail/ConfirmationAccount";
            string subject = $"Nieuw Account";
            string body = "<br/> <a href=" + verificatielink + ">Verifier Account</a>";
            SendMail(recipient, subject, body);
        }

    }
}