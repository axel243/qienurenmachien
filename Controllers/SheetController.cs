using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using QienUrenMachien.Models;
using QienUrenMachien.Repositories;

namespace QienUrenMachien.Controllers
{
    public class SheetController : Controller
    {
        private readonly ITimeSheetRepository repo;

        public SheetController(ITimeSheetRepository repo)
        {
            this.repo = repo;
        }
        public IActionResult Index()
        {
            return View("Month");
        }

        public IActionResult TimeSheet(int Year, int Month)
        {
            var test = repo.GetTimeSheets();
            ViewBag.Test = test;
            var jsonString = JsonConvert.SerializeObject(repo.GetAllDaysInMonth(Year, Month));
            TimeSheet timesheet = new TimeSheet();
            timesheet.Data = jsonString;
            var result = repo.GetAllDaysInMonth(Year, Month);
            return View(result);
        }

        //public ActionResult AddSheet()
        //{
        //    List<Day> listDays = repo.GetAllDaysInMonth(2019, 1);                               //maand en jaar is nog hardcoded
        //    Dictionary<int, DaySheet> testobj = new Dictionary<int, DaySheet> { };
        //    List<DaySheet> listDaySheets = new List<DaySheet>();
            
        //    for (int i = 0; i < listDays.Count; i++)
        //    {
        //        listDaySheets.Add(new DaySheet { Day = i + 1, Month = listDays[i].ToString() });
        //    }

        //    for (int x = 0; x < listDaySheets.Count; x++)
        //    {
        //        testobj.Add(x + 1, listDaySheets[x]);
        //    }

        //    var jsonobj = Newtonsoft.Json.JsonConvert.SerializeObject(testobj);
        //    return View(new TimeSheet { Data = jsonobj });
        //}

        [HttpPost]
        public ActionResult AddSheet(TimeSheet model)
        {
            if (!ModelState.IsValid)
                return View(model);
            repo.AddNewSheet(model);                             // method hoort in de repository 
            return RedirectToAction("Index");
        }
    }

}
