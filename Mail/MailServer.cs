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
using Microsoft.AspNetCore.Identity;

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

        public string Convert(string month)
        {
            return QienUrenMachien.Translation.Translator.TranslateMonth(month);

        }

        public void SendConfirmationMail(string recipient, string werkgever, string url, string werknemer, string month)
        {
            string subject = $"{werknemer} heeft een urenformulier ingeleverd";
            string body = $"Beste {werkgever},<br/><br>Er is een timesheet ingediend door {werknemer} voor {Convert(month)}. Deze timesheet dient door u te worden gekeurd, dat kan via de volgende link: {url}<br><br>Met vriendelijke groet,<br>QienUrenMachien";

            SendMail(recipient, subject, body);
        }

        public void SendApprovalMail(string recipient, string username, string month, string state)
        {
            string body = $"Beste {username},<br><br>Je timesheet voor {month} is {state}.<br><br>Met vriendelijke groet,<br>QienUrenMachien"; 
            string subject = $"Timesheet {state}";

            SendMail(recipient, subject, body);
        }

        public void OpenTimeSheetMail(string recipient, string username, string month, string url)
        {
            string subject = $"Timesheet Heropend";
            string body = $"Beste {username},<br><br>Je timesheet voor {Convert(month)} is opnieuw geopend.<br>https://localhost:44398/Sheet/Overview/{url}<br><br>Met vriendelijke groet,<br>QienUrenMachien";

            SendMail(recipient, subject, body);
        }

        public void AdminRejectedTimeSheet(string recipient, string url, string username, string adminname, string month)
        {
            string subject = $"Timesheet voor {username} is afgewezen";
            string body = $"Beste {adminname},<br><br>De timesheet van {username} voor {Convert(month)} is afgewezen.<br>https://localhost:44398/Sheet/RejectedTimeSheet/{url}<br><br>Met vriendelijke groet,<br>QienUrenMachien";

            SendMail(recipient, subject, body);
        }



        public void SendForgotPasswordMail(string recipient, string personName,  string resetlink)
        {
            string subject = $"Link om het wachtwoord te resetten";
            string aanhef = $"Beste {personName},<br><br>";
            string body1 = "Er is zojuist een verzoek ingediend om uw wachtwoord te resetten. Hieronder kunt u klikken op wachtwoord resetten, waarmee u wordt doorgelinkt naar de pagina om een nieuw wachtwoord op te geven. ";
            string body2 = "<br/> <a href=" + resetlink + ">Wachtwoord resetten</a>";
            string body3 = "Heeft u dit verzoek niet gedaan, dan is er waarschijnlijk iemand anders geweest die dit heeft gedaan. Wij raden u aan om ten alle tijden een sterk wachtwoord te gebruiken en deze met niemand te delen. Mocht dit verzoek vaker voorkomen, dan kunt u hier een melding van maken. ";
            string groet = "<br><br>Met vriendelijke groet,<br>QienUrenMachien";

            SendMail(recipient, subject, aanhef + body1 + body2 + body3 + groet);
        }

        public void SendEditedProfileMail(string recipient, string adminName, string personFirstName, string personLastName, string userId)
        {
            string subject = $"Verzoek profielwijziging van {personFirstName[0]}. {personLastName}";
            string link = "<a href=" + "https://localhost:44398/profile/confirmprofile/" + userId + ">hier</a>";
            string body = @$"Beste {adminName},<br><br> Het profiel van {personFirstName[0]}. {personLastName} is gewijzigd en is in afwachting van goedkeuring.<br> Dit profielverzoek kunt u bekijken door " + link + " te klikken.<br> Mocht dit problemen opleveren, dan kunt u deze zien in de admin dashboard onder het tab profielverzoeken. <br><br>Met vriendelijke groet,<br>QienUrenMachien";

            SendMail(recipient, subject, body);
        }
        public void SendAcceptedProfileMail(string recipient, string personFirstName, string personLastName, string adminFirstName, string adminLastName)
        {
            string subject = "Profielwijziging is goedgekeurd";
            string aanhef = $"Beste {personFirstName},<br><br>";
            string body = $"Je hebt eerder je profielgegevens gewijzigd, deze wijziging is zojuist goedgekeurd door {adminFirstName[0]}. {adminLastName} ";
            string groet = "<br><br>Met vriendelijke groet,<br>QienUrenMachien";

            SendMail(recipient, subject, aanhef + body + groet);
        }

        public void SendDeclinedProfileMail(string recipient, string personFirstName, string personLastName, string adminFirstName, string adminLastName)
        {
            string subject = "Profielwijziging is afgewezen";
            string aanhef = $"Beste {personFirstName},<br><br>";
            string body = $"Je hebt eerder je profielgegevens gewijzigd, deze wijziging is zojuist afgewezen door {adminFirstName[0]}. {adminLastName}. Neem contact op met {adminFirstName} om dit te bespreken.";
            string groet = "<br><br>Met vriendelijke groet,<br>QienUrenMachien";

            SendMail(recipient, subject, aanhef + body + groet);
        }

        public void SendRegisterUserMail(string recipient, string password, string name)
        {
            //string verificatielink = "https://localhost:44398/Mail/ConfirmationAccount";
            string subject = $"Nieuw Account";
            string body = $"<p> Beste {name},<br><br>Er is voor u een nieuw account gemaakt op QienUrenMachien.<br/>U krijgt voor dit account een automatisch gegenereerd wachtwoord die u de eerste keer moet gebruiken om in te loggen. Daarna kunt u het wachtwoord veranderen in de instellingen van uw profiel.<br>Uw wachtwoord is: {password}<br><br>Met vriendelijke groet,<br>QienUrenMachien";
            SendMail(recipient, subject, body);
        }

    }
}