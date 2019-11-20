using QienUrenMachien.Models;
using System.Collections.Generic;

namespace QienUrenMachien.Repositories
{
    public interface ITimeSheetRepository
    {
        TimeSheet GetOneTimeSheet(int SheetID, string UserId);
        List<TimeSheet> GetAllTimeSheets();
    }
}