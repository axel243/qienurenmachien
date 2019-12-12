using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QienUrenMachien.Models;
using QienUrenMachien.Repositories;

namespace QienUrenMachien.Controllers
{
    [Route("api/[controller]")]
    public class DataController : Controller
    {
        private readonly ITimeSheetRepository repo;

        
        public DataController(ITimeSheetRepository repo)
        {
            this.repo = repo;
        }
        [HttpGet("total")]
        public async Task<string> List()
        {
            return await repo.TimeSheetData();
        }
        [HttpGet("days/{url}")]
        public async Task<string> getDays(string url )
        {
            return await repo.DaysData(url);
            
        }
    }
}