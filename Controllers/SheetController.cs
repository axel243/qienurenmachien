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
        public List<Day> listDays;
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
            //var test = repo.GetTimeSheets();
            var result = GetAllDaysInMonth(Year, Month);
            ViewBag.Year = Year;
            ViewBag.Month = Month;
            return View(result);
        }

        public List<Day> GetAllDaysInMonth(int Year, int Month)
        {
            List<Day> days = new List<Day>();
            for (int i = 1; i <= DateTime.DaysInMonth(Year, Month); i++)
            {
                days.Add(new Day(new DateTime(Year, Month, i), null, 0, 0, 0, 0, 0, 0, null));
            }
            //listDays.Add(new Day(new DateTime(Year, Month, 1), "Nos", 4, 4, 2, 5, 7, 0, "None"));
            //listDays.Add(new Day(new DateTime(Year, Month, 2), "Qien", 4, 4, 2, 5, 7, 0, "Eerder weggegaan"));
            listDays = days;
            return days;

        }

        public string DaysToData(int Year, int Month)
        {
            List<Day> listDays = GetAllDaysInMonth(Year, Month);
            var jsonString = JsonConvert.SerializeObject(listDays);
            return jsonString;
        }

        [HttpPost]
        public ActionResult AddSheet(List<Day> listDaysTotal, int Year, int Month)
        {
            TimeSheet timesheet = new TimeSheet();
            listDaysTotal = GetAllDaysInMonth(Year, Month);
            if (!ModelState.IsValid)
                return View(timesheet);
            timesheet.Data = DaysToData(Year, Month);
            timesheet.Month = Month;
            timesheet.ProjectHours = listDaysTotal.Sum(d => d.ProjectHours);
            timesheet.Overwork = listDaysTotal.Sum(d => d.Overwork);
            timesheet.Sick = listDaysTotal.Sum(d => d.Sick);
            timesheet.Absence = listDaysTotal.Sum(d => d.Absence);
            timesheet.Training = listDaysTotal.Sum(d => d.Training);
            timesheet.Other = listDaysTotal.Sum(d => d.Other);
            timesheet.Submitted = true;
            repo.AddNewSheet(timesheet);
            return RedirectToAction("Month");
        }

    }
}
