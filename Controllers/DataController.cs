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
        [HttpGet("")]
        public async Task<string> List(string username)
        {
            return await repo.TimeSheetData();
        }
    }
}