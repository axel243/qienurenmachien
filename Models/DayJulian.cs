using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QienUrenMachien.Models
{
    public class DayJulian
    {
        public DayJulian()
        {
            ProjectHours = 0;
            Overwork = 0;
            Sick = 0;
            Absence = 0;
            Training = 0;
            Other = 0;
        }

        public double ProjectHours { get; set; }
        public double Overwork { get; set; }
        public double Sick { get; set; }
        public double Absence { get; set; }
        public double Training { get; set; }
        public double Other { get; set; }
    }
}
