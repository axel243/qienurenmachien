﻿using QienUrenMachien.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QienUrenMachien.Models
{
    public class FileUploadModel : FileUpload
    {
        public FileViewModel FileView { get; set; }
    }
}
