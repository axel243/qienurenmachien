using QienUrenMachien.Entities;
using QienUrenMachien.Models;
using System.Collections.Generic;

namespace QienUrenMachien.Repositories
{
    public interface IFileUploadRepository
    {
        void UploadFile(ApplicationUser user, string filePath);
        void UploadFile(ApplicationUser user, string filePath, int sheetID);
        List<FileUploadModel> GetFilesByUserId(string userId);
        List<FileUploadModel> GetOtherFilesByUserId(string userId);
        List<FileUploadModel> GetSheetFilesBySheetId(int sheetId);
    }
}