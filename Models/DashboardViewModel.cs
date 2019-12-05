using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QienUrenMachien.Models
{
    public class DashboardViewModel
    {
        public List<ActivityLogViewModel> activityLogViewModels { get; set; }
        public List<TimeSheetWithUser> timeSheetWithUsers { get; set; }
    }
}
