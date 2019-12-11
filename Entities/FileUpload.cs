using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QienUrenMachien.Entities
{
    public class FileUpload
    {
        [Key]
        public int FileId { get; set; }
        [ForeignKey("Id")]
        public ApplicationUser applicationUser { get; set; }
        public string Id { get; set; }
        [ForeignKey("SheetID")]
        public TimeSheet timeSheet { get; set; }
        public int SheetID { get; set; }
        public string FilePath { get; set; }
    }
}
