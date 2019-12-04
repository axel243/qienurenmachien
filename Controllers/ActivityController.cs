using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using QienUrenMachien.Hubs;
using QienUrenMachien.Repositories;

namespace QienUrenMachien.Controllers
{
    public class ActivityController : Controller
    {
        private readonly IHubContext<ChatHub> hub;
       
        private readonly IActivityLogRepository repox;

        public ActivityController(IHubContext<ChatHub> hub, IActivityLogRepository repox)
        {
            this.hub = hub;
            this.repox = repox;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
       
            var logs = repox.GetActivityLogs();
            logs.Reverse();

            await hub.Clients.All.SendAsync("ReceiveMessage1", "testberichtje");

            return View(@"~/Views/Home/Activity.cshtml", logs);
        }
    }
}