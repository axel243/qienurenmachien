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

            return View("Month");
        }
        public IActionResult TimeSheet(int Year, int Month)
        {
            var result = GetAllDaysInMonth(Year, Month);
            return View(result);
        }

        public Dictionary<int, Day> GetAllDaysInMonth(int year, int month)
        {
            Dictionary<int, Day> listDaysDictionary = new Dictionary<int, Day> { };
            //for (int i = 1; i <= DateTime.DaysInMonth(year, month); i++)
            //{
            //    days.Add(new Day(new DateTime(year, month, i), null , 0, 0, 0, 0, 0, 0, null));
            //}
            listDaysDictionary.Add(1,new Day(new DateTime(year, month, 1), "Nos", 4, 4, 2, 5, 7, 0, "None"));
            listDaysDictionary.Add(2,new Day(new DateTime(year, month, 2), "Qien", 4, 4, 2, 5, 7, 0, "Eerder weggegaan"));
            return listDaysDictionary;
            
        }

        public String DaysToData(int Year, int Month)
        {
            Dictionary<int, Day> DictionarySerialize = GetAllDaysInMonth(Year, Month);
            var jsonString = JsonConvert.SerializeObject(DictionarySerialize);
            TimeSheet timesheet = new TimeSheet();
            timesheet.Data = jsonString;
            return timesheet.Data;
        }

        public TimeSheet DaysToSheet(int Year, int Month)
        {
            Dictionary<int, Day> listDaysDictionary = GetAllDaysInMonth(Year, Month);
            TimeSheet timesheet = new TimeSheet();
            timesheet.Data = DaysToData(Year, Month);
            timesheet.ProjectHours = listDaysDictionary.Sum(d => d.Value.ProjectHours);
            timesheet.Overwork = listDaysDictionary.Sum(d => d.Value.Overwork);
            timesheet.Sick = listDaysDictionary.Sum(d => d.Value.Sick);
            timesheet.Absence = listDaysDictionary.Sum(d => d.Value.Absence);
            timesheet.Training = listDaysDictionary.Sum(d => d.Value.Training);
            timesheet.Other = listDaysDictionary.Sum(d => d.Value.Other);
            timesheet.Submitted = true;
            return timesheet;
        }

           [HttpPost]
           public ActionResult AddSheet(TimeSheet model)
           {
               if (!ModelState.IsValid)
                   return View(model);
                model = DaysToSheet(2019, 1);                          
               return RedirectToAction("Index");
           }
    }
}
