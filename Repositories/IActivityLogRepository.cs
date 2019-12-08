using QienUrenMachien.Entities;
using QienUrenMachien.Models;
using System.Collections.Generic;

namespace QienUrenMachien.Repositories
{
    public interface IActivityLogRepository
    {
        void LogActivity(ApplicationUser user, string activity, string comment);
        List<ActivityLogViewModel> GetActivityLogs();
        ActivityLogViewModel GetLastLog();

    }
}