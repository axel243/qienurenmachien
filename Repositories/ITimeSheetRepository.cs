using Microsoft.AspNetCore.Mvc.Rendering;
using QienUrenMachien.Entities;
using QienUrenMachien.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QienUrenMachien.Repositories
{
    public interface ITimeSheetRepository
    {

        Task<TimeSheet> UpdateTimeSheet(TimeSheet _timeSheet);

        Task<TimeSheet> GetTimeSheet(int Id);

        TimeSheetViewModel GetOneTimeSheet(int SheetID);
        Task<List<TimeSheet>> GetAllTraineeTimeSheets(TimeSheetsViewModel model);
        Task<List<TimeSheet>> GetAllEmployeeTimeSheets(TimeSheetsViewModel model);
        List<SelectListItem> GetMonths();
        TimeSheet GetOneTimeSheet(string url);
    }
}