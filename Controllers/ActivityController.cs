using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QienUrenMachien.Repositories;

namespace QienUrenMachien.Controllers
{
    public class ActivityController : Controller
    {
        private readonly IActivityLogRepository repox;

        public ActivityController(IActivityLogRepository repox)
        {
            this.repox = repox;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var logs = repox.GetActivityLogs();
            return View(@"~/Views/Home/Activity.cshtml", logs);
        }
    }
}