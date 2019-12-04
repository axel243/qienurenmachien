using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QienUrenMachien.Models;

namespace QienUrenMachien.Controllers
{
    [Route("api/[controller]")]
    public class DataController : Controller
    {
        [HttpGet("")]
        public IEnumerable<Note> List(string username)
        {
            return new List<Note>{
                new Note{Title = "Shopping list", Contents="Some Apples"},
                new Note{Title = "Thoughts on .NET Core", Contents="To follow..."}
             };
        }
    }
}