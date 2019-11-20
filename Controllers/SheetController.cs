using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QienUrenMachien.Data;
using QienUrenMachien.Models;

namespace QienUrenMachien.Controllers
{
    public class SheetController : Controller
    {
        private RepositoryContext repo;

        public SheetController(RepositoryContext repo)
        {
            this.repo = repo;
        }
        public IActionResult Index()
        {


            return View("Month");
        }
        public IActionResult TimeSheet(int Year, int Month)
        {
            ViewBag.Year = Year;
            ViewBag.Month = Month;
            var test = GetTimeSheets();
            ViewBag.Test = test;


            var result = GetAllDaysInMonth(Year, Month);
            return View(result);
        }

        public List<DateTime> GetAllDaysInMonth(int year, int month)
        {
            var ret = new List<DateTime>();
            for (int i = 1; i <= DateTime.DaysInMonth(year, month); i++)
            {
                ret.Add(new DateTime(year, month, i));
            }
            return ret;
        }

        public ActionResult AddSheet()
        {
            List<DateTime> listDays = GetAllDaysInMonth(2019, 1);                               //maand en jaar is nog hardcoded
            Dictionary<int, DaySheet> testobj = new Dictionary<int, DaySheet> { };
            List<DaySheet> listDaySheets = new List<DaySheet>();

            for (int i = 0; i < listDays.Count; i++)
            {
                listDaySheets.Add(new DaySheet { Day = i + 1, Month = listDays[i].ToString() });
            }


            for (int x = 0; x < listDaySheets.Count; x++)
            {
                testobj.Add(x + 1, listDaySheets[x]);
            }

            var jsonobj = Newtonsoft.Json.JsonConvert.SerializeObject(testobj);

            return View(new TimeSheet { Data = jsonobj });
        }

        [HttpPost]
        public ActionResult AddSheet(TimeSheet model)
        {
            if (!ModelState.IsValid)
                return View(model);
            AddNewSheet(model);                             // method hoort in de repository 
            return RedirectToAction("Index");
        }

        public void AddNewSheet(TimeSheet timeSheetModel)
        {
            repo.TimeSheets.Add(new TimeSheet
            {
                Project = timeSheetModel.Project,
                Month = timeSheetModel.Month,
                ProjectHours = timeSheetModel.ProjectHours,
                Overwork = timeSheetModel.Overwork,
                Sick = timeSheetModel.Sick,
                Absence = timeSheetModel.Absence,
                Training = timeSheetModel.Training,
                Other = timeSheetModel.Other,
                Data = timeSheetModel.Data
                
            });
            repo.SaveChanges();
        }
        public List<TimeSheet> GetTimeSheets()
        {
            var sheetList = repo.TimeSheets.Select(n => new TimeSheet
            {
                SheetID = n.SheetID,
                Project = n.Project,
                Month = n.Month,
                ProjectHours = n.ProjectHours,
                Overwork = n.Overwork,
                Sick = n.Sick,
                Absence = n.Absence,
                Training = n.Training,
                Other = n.Other,
                Data = n.Data
            }).ToList();
            return sheetList;
        }
    }
}
