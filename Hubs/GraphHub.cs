using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using QienUrenMachien.Models;
using QienUrenMachien.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QienUrenMachien.Hubs
{
    public class GraphHub : Hub
    {
        private readonly ITimeSheetRepository repo;

        public GraphHub(ITimeSheetRepository repo)
        {
            this.repo = repo;
        }

        public async Task SendMessage(int SheetID)
        {
            Console.WriteLine(SheetID);

            var _timeSheet = await repo.GetTimeSheet(SheetID);

            //   double projectHours = _timeSheet.ProjectHours;
            //   double overwork = _timeSheet.Overwork;
            //   double sick = _timeSheet.Sick;
            //   double absence = _timeSheet.Absence;
            //   double training = _timeSheet.Training;
            //   double other = _timeSheet.Other;

            //_timeSheet.ProjectHours = projectHours;
            //_timeSheet.Overwork = overwork;
            //_timeSheet.Sick = sick;
            //_timeSheet.Absence = absence;
            //_timeSheet.Training = training;
            //_timeSheet.Other = other;

            var DoughnutUpdate = new
            {
                ProjectHours = _timeSheet.ProjectHours,
                Overwork = _timeSheet.Overwork,
                Sick = _timeSheet.Sick,
                Absence = _timeSheet.Absence,
                Training = _timeSheet.Training,
                Other = _timeSheet.Other,

            };

            await Clients.Caller.SendAsync("ReceiveMessage2", JsonConvert.SerializeObject(DoughnutUpdate));
        }

        public async Task SendActivity()
        {
            await Clients.All.SendAsync("ReceiveMessage1", "teststring");
        }

    }
}
