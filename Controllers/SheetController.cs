using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using QienUrenMachien.Data;
using QienUrenMachien.Models;
using QienUrenMachien.Repositories;

namespace QienUrenMachien.Controllers
{
    public class SheetController : Controller
    {
        private readonly ITimeSheetRepository repo;
        TimeSheet timesheet = new TimeSheet();
        public SheetController(ITimeSheetRepository repo)
        {
            this.repo = repo;
        }
        public IActionResult Month()
        {
            return View("Month");
        }

        public IActionResult TimeSheet(int Year, int Month)
        {
            timesheet.Year = Year;
            timesheet.Month = Month;
            var result = SetAllDaysInMonth();
            return View(result);
        }

        public TimeSheet SetAllDaysInMonth()
        {
            List<Day> days = new List<Day>();
            for (int i = 1; i <= DateTime.DaysInMonth(timesheet.Year, timesheet.Month); i++)
            {
                days.Add(new Day(new DateTime(timesheet.Year, timesheet.Month, i), null, 0, 0, 0, 0, 0, 0, null));
            }
            timesheet.days = days;
            return timesheet;

        }

        public string DaysToData()
        {
            var jsonString = JsonConvert.SerializeObject(timesheet.days);
            return jsonString;
        }

       
        [HttpPost]
        public ActionResult AddSheet(TimeSheet timesheet)
        {
            this.timesheet = timesheet;
            if (!ModelState.IsValid)
                return View(timesheet);
            timesheet.Data = DaysToData();
            timesheet.ProjectHours = timesheet.days.Sum(d => d.ProjectHours);
            timesheet.Overwork = timesheet.days.Sum(d => d.Overwork);
            timesheet.Sick = timesheet.days.Sum(d => d.Sick);
            timesheet.Absence = timesheet.days.Sum(d => d.Absence);
            timesheet.Training = timesheet.days.Sum(d => d.Training);
            timesheet.Other = timesheet.days.Sum(d => d.Other);
            timesheet.Submitted = 1;
            repo.AddNewSheet(timesheet);
            return RedirectToAction("Month");
        }

        //public object DataToDays()
        //{
        //    timesheet = repo.GetOneTimeSheet(12);
        //    var jsonString = JsonConvert.DeserializeObject(timesheet.Data);
        //    return jsonString;
        //}

    }
}
