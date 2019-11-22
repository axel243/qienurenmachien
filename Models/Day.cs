using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QienUrenMachien.Models
{
    public class Day
    {
        public Day()
        {
            projecthours = 0;
            overwork = 0;
            sick = 0;
            absence = 0;
            training = 0;
            other = 0;

        }

        public double projecthours { get; set; }
        public double overwork { get; set; }
        public double sick { get; set; }
        public double absence { get; set; }
        public double training { get; set; }
        public double other { get; set; }
    }
}
