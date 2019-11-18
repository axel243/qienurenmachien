using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QienUrenMachien.Models
{
    public enum Status { New, ForApproval, Approved, NotApproved }
    public class TimeSheet
    {

        [Key]
        public int SheetID { get; set; }
        public string Project { get; set; }
        public string Month { get; set; }
        public double ProjectHours { get; set; }
        public double Overwork { get; set; }
        public double Sick { get; set; }
        public double Absence { get; set; }
        public double Training { get; set; }
        public double Other { get; set; }
        public Status Status { get; set; }
        public string Data { get; set; }


    }
}
