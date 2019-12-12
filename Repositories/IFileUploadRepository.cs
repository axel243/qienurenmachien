using QienUrenMachien.Entities;
using QienUrenMachien.Models;
using System.Collections.Generic;

namespace QienUrenMachien.Repositories
{
    public interface IFileUploadRepository
    {
        void UploadFile(ApplicationUser user, string filePath);
        List<FileUploadModel> GetFiles();
        List<FileUploadModel> GetFilesByUserId(string userId);
    }
}