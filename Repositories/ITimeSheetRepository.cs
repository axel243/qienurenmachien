using QienUrenMachien.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QienUrenMachien.Repositories
{
    public interface ITimeSheetRepository
    {
        TimeSheet GetOneTimeSheet(int SheetID, string UserId);

        Task<TimeSheet> UpdateTimeSheet(TimeSheet _timeSheet);

        Task<TimeSheet> GetTimeSheet(int Id);
        List<TimeSheet> GetAllTimeSheets();
    }
}