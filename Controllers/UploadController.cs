using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QienUrenMachien.Entities;
using QienUrenMachien.Models;
using QienUrenMachien.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace QienUrenMachien.Controllers
{
    public class UploadController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IFileUploadRepository fileRepo;

        public UploadController(UserManager<ApplicationUser> userManager, IFileUploadRepository fileRepo)
        {
            this.userManager = userManager;
            this.fileRepo = fileRepo;
        }


        [HttpGet]
        public IActionResult SheetAttachment(string url, int sheetID)
        {
            //url en sheetId wordt opgehaald uit de submitTimeSheet action in de sheet controller
            //deze worden in de view gebruikt om de geuploade bestanden aan de sheetID te linken
            var file = new FileSheetUploadViewModel
            {
                url = url,
                sheetID = sheetID
            };

            return View(@"~/Views/Attachments/SheetAttachments.cshtml", file);
        }

        public IActionResult Index()
        {
            //userid van ingelogde gebruiker wordt opgehaald
            var userid = userManager.GetUserId(HttpContext.User);

            //userid wordt gebruikt om de bestanden van deze user op te halen
            var files = fileRepo.GetFilesByUserId(userid);

            return View(@"~/Views/Attachments/Files.cshtml", files);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult ViewUserFiles(string userId)
        {
            //userid wordt mee gegeven uit de view ViewUser
            //hiermee worden de bestanden van deze user opgehaald
            var files = fileRepo.GetFilesByUserId(userId);
            return View(@"~/Views/Attachments/Files.cshtml", files);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitFiles(FileViewModel model)
        {
            if (model.Files != null)
            {
                //userid van ingelogde gebruiker wordt opgehaald
                var userid = userManager.GetUserId(HttpContext.User);
                //deze userid wordt gebruikt om de gebruiker object op te halen
                var currentUser = await userManager.FindByIdAsync(userid);

                //de file wordt middels het model doorgegeven en opgeslagen met koppeling naar de user
                UploadFile(currentUser, model);

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> SubmitSheetFiles(FileSheetUploadViewModel model)
        {
            //deze action heeft betrekking tot het uploaden van bestanden die betrekking hebben de in te vullen uren formulier, bijv declaratieformulier
            //check of er wel bestanden de view worden geupload 
            if (model.Files != null)
            {
                //userid van ingelogde gebruiker wordt opgehaald
                var userid = userManager.GetUserId(HttpContext.User);
                //deze userid wordt gebruikt om de gebruiker object op te halen
                var currentUser = await userManager.FindByIdAsync(userid);

                //de file wordt middels het model doorgegeven en opgeslagen met koppeling naar de user
                UploadSheetFile(currentUser, model);

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }


        void UploadFile(ApplicationUser user, FileViewModel model)
        {
            // algemene bestanden zoals persoonlijke ontwikkeling
            // voor elk bestand dat wordt geupload wordt deze loop uitgevoerd
            foreach (var file in model.Files)
            {
                //de pad van de folder, waar het bestand wordt opgeslagen
                var uploadPath = $@"wwwroot/Uploads/Attachments/";

                //het bestaan van de pad hierboven wordt gecontroleerd en als die niet bestaat worden de mappen aangemaakt
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }
                var date = DateTime.Now.ToString("ddMMyyyy");

                //de volledige bestandsnaam wordt gesplitst in naam en extensie
                var fileExtension = Path.GetExtension(file.FileName);
                var fileNoExtension = Path.GetFileNameWithoutExtension(file.FileName);

                //de volledige bestandsnaam krijgt een andere naam
                var fileName = $"{user.Firstname}_{user.Lastname}_{date}_{fileNoExtension}";

                //hier wordt de volledige pad samengesteld en kopie opgeslagen van het bestand dat wordt geupload
                using (var fileStream = new FileStream(Path.Combine(uploadPath, fileName + fileExtension), FileMode.Create, FileAccess.Write))
                {
                    file.CopyTo(fileStream);
                }

                //volledige pad naar bestand
                var filePath = $@"~/Uploads/Attachments/" + fileName + fileExtension;

                //referentie(pad) naar het bestand wordt opgeslagen in de database
                fileRepo.UploadFile(user, filePath);

            }
        }
        void UploadSheetFile(ApplicationUser user, FileSheetUploadViewModel model)
        {
            // bestanden die betrekking hebben op het urenformulier, zoals declaratieformulier, onkosten
            // voor elk bestand dat wordt geupload wordt deze loop uitgevoerd
            foreach (var file in model.Files)
            {
                //de pad van de folder, waar het bestand wordt opgeslagen
                var uploadPath = $@"wwwroot/Uploads/Attachments/";

                //het bestaan van de pad hierboven wordt gecontroleerd en als die niet bestaat worden de mappen aangemaakt
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }
                var date = DateTime.Now.ToString("ddMMyyyy");

                //de volledige bestandsnaam wordt gesplitst in naam en extensie
                var fileExtension = Path.GetExtension(file.FileName);
                var fileNoExtension = Path.GetFileNameWithoutExtension(file.FileName);

                //de volledige bestandsnaam krijgt een andere naam
                var fileName = $"{user.Firstname}_{user.Lastname}_{date}_{fileNoExtension}";

                //hier wordt de volledige pad samengesteld en kopie opgeslagen van het bestand dat wordt geupload
                using (var fileStream = new FileStream(Path.Combine(uploadPath, fileName + fileExtension), FileMode.Create, FileAccess.Write))
                {
                    file.CopyTo(fileStream);
                }

                //volledige pad naar bestand
                var filePath = $@"~/Uploads/Attachments/" + fileName + fileExtension;

                //referentie(pad) naar het bestand wordt opgeslagen in de database
                fileRepo.UploadFile(user, filePath, model.sheetID);
            }
        }

        public IActionResult DownloadDocument(string filePath)
        {

            var fileName = Path.GetFileName(filePath);

            string fileRePath = filePath.Replace("~", "wwwroot");

            byte[] fileBytes = System.IO.File.ReadAllBytes(fileRePath);

            return File(fileBytes, "application/force-download", fileName);

        }
    }
}
