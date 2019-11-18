using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QienUrenMachien.Models;

namespace QienUrenMachien.Controllers
{
    public class SheetController : Controller
    {
        public IActionResult Index()
        {
            return View("Month");
        }
        public IActionResult TimeSheet(int Year, int Month)
        {
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
    }
}
