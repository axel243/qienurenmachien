using QienUrenMachien.Entities;

namespace QienUrenMachien.Repositories
{
    public interface IActivityLogRepository
    {
        void LogActivity(ApplicationUser user, string activity, string comment);
    }
}