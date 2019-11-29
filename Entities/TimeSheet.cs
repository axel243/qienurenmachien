using QienUrenMachien.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QienUrenMachien.Entities
{
    public class TimeSheet
    {

        [Key]
        public int SheetID { get; set; }

        [ForeignKey("Id")]
        public ApplicationUser applicationUser { get; set; }
        public string Id {get; set;}
        public string Project {get; set;}
        public string Month { get; set; }
        [NotMapped]
        public int Year { get; set; }
        public double ProjectHours { get; set; }
        public double Overwork { get; set; }
        public double Sick { get; set; }
        public double Absence { get; set; }
        public double Training { get; set; }
        public double Other { get; set; }
        public bool Submitted { get; set; }
        public string Approved {get; set;}
        public string Data { get; set; }
        [NotMapped]
        public List<Day> days { get; set; }
        public string Comment { get; set; }

        public string Url { get; set; }

        public DateTime theDate { get; set; }
    }
}
