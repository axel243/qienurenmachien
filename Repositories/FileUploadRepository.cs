
using Microsoft.AspNetCore.Identity;
using QienUrenMachien.Data;
using QienUrenMachien.Entities;
using QienUrenMachien.Models;
using System.Collections.Generic;
using System.Linq;

namespace QienUrenMachien.Repositories
{
    public class FileUploadRepository : IFileUploadRepository
    {
        private readonly RepositoryContext context;
        private readonly UserManager<ApplicationUser> userManager;


        public FileUploadRepository(RepositoryContext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;

        }

        public void UploadFile(ApplicationUser user, string filePath)
        {


            FileUpload file = new FileUpload();

            file.Id = user.Id;
            file.SheetID = 50; //null
            file.FilePath = filePath;
            
            context.FileUploads.Add(file);
            context.SaveChanges();
        }

        public void UploadFile(ApplicationUser user, string filePath, int sheetID)
        {


            FileUpload file = new FileUpload();

            file.Id = user.Id;
            file.SheetID = sheetID;
            file.FilePath = filePath;

            context.FileUploads.Add(file);
            context.SaveChanges();
        }

        public List<FileUploadModel> GetFiles()
        {

            var files = context.FileUploads.
                Select(f => new FileUploadModel
                {
                FileId = f.FileId,
                applicationUser = f.applicationUser,
                Id = f.Id,
                SheetID = f.SheetID,
                FilePath = f.FilePath
                }).ToList();

            return files;
        }

        public List<FileUploadModel> GetFilesByUserId(string userId)
        {
                var files = context.FileUploads.Where(i => i.Id == userId).
                Select(f => new FileUploadModel
                {
                FileId = f.FileId,
                applicationUser = f.applicationUser,
                Id = f.Id,
                SheetID = f.SheetID,
                FilePath = f.FilePath
                }).ToList();

            return files;

        }
    }
}