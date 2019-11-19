using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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
        public IActionResult Month()
        {

            return View();
        }
        public IActionResult TimeSheet(int Year, int Month)
        {

            var test = GetTimeSheets();
            ViewBag.Test = test;
            var jsonString = JsonConvert.SerializeObject(GetAllDaysInMonth(Year, Month));
            TimeSheet timesheet = new TimeSheet();
            timesheet.Data = jsonString;
            var result = GetAllDaysInMonth(Year, Month);
            return View(result);
        }

        public List<Day> GetAllDaysInMonth(int year, int month)
        {
            var days = new List<Day>();
            days.Add(new Day(new DateTime(year, month, 1), "Nos", 4, 4, 2, 5, 7, 0, "None"));
            days.Add(new Day(new DateTime(year, month, 2), "Qien", 4, 4, 2, 5, 7, 0, "Eerder weggegaan"));
            return days;
            
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
