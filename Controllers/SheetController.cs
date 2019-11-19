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

            var sheet1 = GetTimeSheets();
            var sheet1DATA = sheet1[0].Data;
            var json = Newtonsoft.Json.JsonConvert.DeserializeObject<TimeSheet>(sheet1DATA);
            //            {
            //                "1": { "SheetID": 3, "Project": "bla", "Month": "januari", "ProjectHours": 10, "Overwork": 15, "Sick": 20, "Absence": 25, "Training": 30, "Other": 35, "Status": 1 },
            //                "2": { "SheetID": 3, "Project": "macaw", "Month": "januari", "ProjectHours": 11, "Overwork": 16, "Sick": 17, "Absence": 18, "Training": 19, "Other": 20, "Status": 1 }
            //            }


            return View("Month");
        }
        public IActionResult TimeSheet(int Year, int Month)
        {

            var test = GetTimeSheets();
            ViewBag.Test = test;


            var result = GetAllDaysInMonth(Year, Month);
            ViewBag.Result = result;
            return View(result);
        }

        public String GetAllDaysInMonth(int year, int month)
        {
            var days = new List<Day>();

            Day daya = new Day(new DateTime(year, month, 1), "Nos", 4, 8, 2, 5, 7, 0, "None");
            Day dayb = new Day(new DateTime(year, month, 2), "Gen25", 7, 8, 3, 9, 0, 0, "asasa");
            days.Add(daya);
            days.Add(dayb);
            /*  for (int i = 1; i <= DateTime.DaysInMonth(year, month); i++)
              {
                  days.Add(new Day(new DateTime(year, month, i), "Nos", 4, 8, 2, 5, 7, 0, "None"));
              }
              return days;*/
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(days);
            return jsonString;

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
