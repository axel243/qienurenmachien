
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;
using System.Text;
using QienUrenMachien.Repositories;
using QienUrenMachien.Entities;
using QienUrenMachien.Models;
using System.Collections.Generic;

namespace QienUrenMachien.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ITimeSheetRepository repo;
        private readonly IActivityLogRepository repox;


        public ChatHub(ITimeSheetRepository repo, IActivityLogRepository repox)
        {
            this.repo = repo;
            this.repox = repox;
        }

        public async Task SendMessage(int SheetID, string data, string comment)
        {
            Console.WriteLine(SheetID);
            Console.WriteLine(data);
            Console.WriteLine(comment);
            var _timeSheet = await repo.GetTimeSheet(SheetID);

            _timeSheet.Data = data;


            var JSONobj = JsonConvert.DeserializeObject<Dictionary<string, DayJulian>>(data);

            double projectHours = 0;
            double overwork = 0;
            double sick = 0;
            double absence = 0;
            double training = 0;
            double other = 0;
           

            foreach (var item in JSONobj)
            {
                projectHours += item.Value.ProjectHours;
                overwork += item.Value.Overwork;
                sick += item.Value.Sick;
                absence += item.Value.Absence;
                training += item.Value.Training;
                other += item.Value.Other;
            }

            _timeSheet.ProjectHours = projectHours;
            _timeSheet.Overwork = overwork;
            _timeSheet.Sick = sick;
            _timeSheet.Absence = absence;
            _timeSheet.Training = training;
            _timeSheet.Other = other;
            _timeSheet.Comment = comment;

            var TableUpdate = new
            {
                Project = _timeSheet.Project,
                Month = _timeSheet.Month,
                ProjectHours = _timeSheet.ProjectHours,
                Overwork = _timeSheet.Overwork,
                Sick = _timeSheet.Sick,
                Absence = _timeSheet.Absence,
                Training = _timeSheet.Training,
                Other = _timeSheet.Other,
                Submitted = _timeSheet.Submitted,
                Approved = _timeSheet.Approved,
                Comment = _timeSheet.Comment

            };

            await Clients.Caller.SendAsync("ReceiveMessage", JsonConvert.SerializeObject(TableUpdate));

            var result = await repo.UpdateTimeSheet(_timeSheet);

        }

        public async Task SendActivity()
        {
            await Clients.All.SendAsync("ReceiveMessage1", "teststring");
        }


        //public  async Task SendMessage1(string message)
        //{
         
        //    await Clients.All.SendAsync("ReceiveMessage1", message);
        //}

    }
}
