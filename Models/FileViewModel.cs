using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QienUrenMachien.Models
{
    public class FileViewModel
    {
        public List<IFormFile> Files { get; set; }
    }
}
