
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;
using System.Text;
using QienUrenMachien.Repositories;
using QienUrenMachien.Entities;

namespace QienUrenMachien.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ITimeSheetRepository repo;

        public ChatHub(ITimeSheetRepository repo){
            this.repo = repo;
        }

        public async Task SendMessage(int SheetID, string data)
        {
            var _timeSheet = await repo.GetTimeSheet(SheetID);

            _timeSheet.Data = data;
            var result = await repo.UpdateTimeSheet(_timeSheet);

        }

    }
}
