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
        public IActionResult SheetAttachment(string url)
        {
            var file = new FileSheetUploadViewModel
            {
                url = url
            };

            return View(@"~/Views/Attachments/Test.cshtml", file);
        }

        public IActionResult Index()
        {
            var files = fileRepo.GetFiles();
            ViewBag.Files = files;

            return View(@"~/Views/Attachments/AddFiles.cshtml");
        }

        //public IActionResult ViewFiles(string userId)
        //{
        //    var files = fileRepo.GetFilesByUserId(userId);
           
        //    return View(@"~/Views/Attachments/ViewFiles.cshtml", files);
        //}

        [HttpPost]
        public async Task<IActionResult> SubmitFiles(FileViewModel model)
        {
            if (model.Files != null)
            {
                var userid = userManager.GetUserId(HttpContext.User);

                var currentUser = await userManager.FindByIdAsync(userid);

                UploadFile(currentUser, model);

                return RedirectToAction("Index");
            }


            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> SubmitSheetFiles(FileSheetUploadViewModel model)
        {
            if (model.Files != null)
            {
                var userid = userManager.GetUserId(HttpContext.User);

                var currentUser = await userManager.FindByIdAsync(userid);

                UploadFile(currentUser, null);

                return RedirectToAction("Index");
            }


            return RedirectToAction("Index");
        }


        void UploadFile(ApplicationUser user, FileViewModel model)
        {
            foreach (var file in model.Files)
            {
                var uploadPath = $@"wwwroot/Uploads/Attachments/";

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }
                var date = DateTime.Now;


                var fileExtension = Path.GetExtension(file.FileName);
                var fileNoExtension = Path.GetFileNameWithoutExtension(file.FileName);
                var fileName = $"{user.Firstname}_{user.Lastname}_{date.ToShortDateString()}_{fileNoExtension}";

                using (var fileStream = new FileStream(Path.Combine(uploadPath, fileName + fileExtension), FileMode.Create, FileAccess.Write))
                {
                    file.CopyTo(fileStream);
                }


                var filePath = $@"~/Uploads/Attachments/" + fileName + fileExtension;

                //referentie(pad) naar het bestand wordt opgeslagen in de database
                fileRepo.UploadFile(user, filePath);

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
