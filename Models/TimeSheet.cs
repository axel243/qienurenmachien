using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QienUrenMachien.Models
{
    public class TimeSheet
    {

        [Key]
        public int SheetID { get; set; }

        [ForeignKey("Id")]
        public ApplicationUser applicationUser { get; set; }
        public string Id {get; set;}
        public double ProjectHours { get; set; }
        public double Overwork { get; set; }
        public double Sick { get; set; }
        public double Absence { get; set; }
        public double Training { get; set; }
        public double Other { get; set; }
        public bool Submitted { get; set; }
        public string Approved {get; set;}
        public string Data { get; set; }
    }
}
