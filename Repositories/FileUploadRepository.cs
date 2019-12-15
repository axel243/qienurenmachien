
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

        //query om regel toe tevoegen in database tabel fileupload met betrekking tot overige bestanden
        public void UploadFile(ApplicationUser user, string filePath)
        {
            FileUpload file = new FileUpload();

            file.Id = user.Id;
            file.SheetID = null;
            file.FilePath = filePath;
            
            context.FileUploads.Add(file);
            context.SaveChanges();
        }

        //query om regel toe tevoegen in database tabel fileupload met betrekking tot een specifieke urenformulier
        public void UploadFile(ApplicationUser user, string filePath, int sheetID)
        {
            FileUpload file = new FileUpload();

            file.Id = user.Id;
            file.SheetID = sheetID;
            file.FilePath = filePath;

            context.FileUploads.Add(file);
            context.SaveChanges();
        }

        //alle files van een specifieke user opvragen
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

        //overige files opvragen van een specifieke user
        public List<FileUploadModel> GetOtherFilesByUserId(string userId)
        {
            var files = context.FileUploads.Where(b => b.Id == userId).Where(c => c.SheetID == null).
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
        //files van specifieke urenformulier opvragen
        public List<FileUploadModel> GetSheetFilesBySheetId(int sheetId)
        {
            var files = context.FileUploads.Where(i => i.SheetID == sheetId).
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