using QienUrenMachien.Models;
using System.Collections.Generic;

namespace QienUrenMachien.Repositories
{
    public interface ITimeSheetRepository
    {
        TimeSheet GetOneTimeSheet(int SheetID);
        List<TimeSheet> GetAllTimeSheets();
        List<TimeSheet> GetTimeSheets();
        void AddNewSheet(TimeSheet timeSheetModel);
    }
}