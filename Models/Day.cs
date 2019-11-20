using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QienUrenMachien.Models
{
    public class Day
    {

        public Day(DateTime date, string project, double projectHours, double overwork, double sick, double absence, double training, double other, string otherExplanation)
        {
            this.date = date;
            Project = project;
            ProjectHours = projectHours;
            Overwork = overwork;
            Sick = sick;
            Absence = absence;
            Training = training;
            Other = other;
            OtherExplanation = otherExplanation;
        }


        // "1": { "SheetID": 3, "Project": "bla", "Month": "januari", "ProjectHours": 10, "Overwork": 15, "Sick": 20, "Absence": 25, "Training": 30, "Other": 35, "Status": 1 },

        public DateTime date { get; set; }
        public string Project { get; set; }
        public double ProjectHours { get; set; }
        public double Overwork { get; set; }
        public double Sick { get; set; }
        public double Absence { get; set; }
        public double Training { get; set; }
        public double Other { get; set; }
        public string OtherExplanation { get; set; }
    }
}
