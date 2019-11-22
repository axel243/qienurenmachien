using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QienUrenMachien.Models
{
    public class TimeSheetsViewModel
    {
        public List<TimeSheet> Trainees { get; set; }
        public List<TimeSheet> Employees { get; set; }
        public string Month { get; set; }
        public List<SelectListItem> Months { get; set; }
    }
}
