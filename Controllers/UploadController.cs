using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QienUrenMachien.Entities;
using QienUrenMachien.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace QienUrenMachien.Controllers
{
    public class UploadController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;

        public UploadController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return View(@"~/Views/Attachments/AddFiles.cshtml");
        }


        [HttpPost]
        public async Task<IActionResult> SubmitFiles(FileViewModel model)
        {
            var userid = userManager.GetUserId(HttpContext.User);

            var currentUser = await userManager.FindByIdAsync(userid);

            UploadFile(currentUser, model);
            
            return View(@"~/Views/Attachments/Succes.cshtml");
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
            }
        }
    }
}