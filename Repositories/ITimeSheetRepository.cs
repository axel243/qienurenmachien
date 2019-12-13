using Microsoft.AspNetCore.Mvc.Rendering;
using QienUrenMachien.Entities;
using QienUrenMachien.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QienUrenMachien.Repositories
{
    public interface ITimeSheetRepository
    {

        Task<TimeSheet> UpdateTimeSheet(TimeSheet _timeSheet);
        Task<TimeSheet> GetTimeSheet(int Id);

        Task<string> DaysData(string url);
        Task<List<TimeSheetWithUser>> GetLastMonthProfileEdits();
        Task<List<TimeSheet>> GetAllTraineeTimeSheets(TimeSheetsViewModel model);
        Task<List<TimeSheet>> GetAllEmployeeTimeSheets(TimeSheetsViewModel model);

        Task<List<TimeSheet>> GetUserOverview(string id, DateTime ActiveFrom, DateTime ActiveUntil);
        List<SelectListItem> GetMonths();
        List<SelectListItem> GetYears();
        TimeSheet GetOneTimeSheet(string url);

        Task<TimeSheetViewModel> GetOneTimeSheetAsync(string Id, int SheetID);
        Task<List<TimeSheetWithUser>> GetTimeSheetAndUser();
        Task<TimeSheetViewModel> GetOneTimeSheetByUrl(string url);
        Task<TimeSheet> GetTimeSheet(string id);

        Task<TimeSheet> GetTimeSheetUrl(string url);

        List<TimeSheet> GetTimeSheets();


        Task<string> TimeSheetData();

        TimeSheet AddTimeSheetTemp();


        TimeSheet AddTimeSheet(string userId, string data);

        Task<string> TimeSheetDataCSV();
    }
}