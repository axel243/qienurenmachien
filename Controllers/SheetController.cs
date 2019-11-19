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
