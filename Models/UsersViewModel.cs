using QienUrenMachien.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QienUrenMachien.Models
{
    public class UsersViewModel
    {
        public IList<ApplicationUser> Trainees { get; set; }
        public IList<ApplicationUser> Employees { get; set; }
        public IList<ApplicationUser> Employers { get; set; }
    }
}
