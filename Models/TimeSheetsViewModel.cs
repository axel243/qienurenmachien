using Microsoft.AspNetCore.Mvc.Rendering;
using QienUrenMachien.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QienUrenMachien.Models
{
    public class TimeSheetsViewModel
    {
        public List<TimeSheet> Trainees { get; set; }
        public List<TimeSheet> Employees { get; set; }
        public List<TimeSheet> sheets { get; set; }
        public string Month { get; set; }
        public List<SelectListItem> Months { get; set; }
        public List<SelectListItem> Years { get; set; }
        public DateTime theDate { get; set; }
    }
}
