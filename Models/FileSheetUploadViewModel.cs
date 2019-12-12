using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QienUrenMachien.Models
{
    public class FileSheetUploadViewModel
    {
        public string url { get; set; }
        public int sheetID { get; set; }
        public List<IFormFile> Files { get; set; }
    }
}
