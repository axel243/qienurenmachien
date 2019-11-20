using QienUrenMachien.Models;
using System.Collections.Generic;

namespace QienUrenMachien.Repositories
{
    public interface ITimeSheetRepository
    {
        TimeSheet GetOneTimeSheet(string id, string Month);
        List<TimeSheet> GetAllTimeSheets();
    }
}