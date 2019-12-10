using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QienUrenMachien.Models
{
    public class TimeSheetWithUser
    {
        public string userId { get; set; }
        public string url { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Status { get; set; }
        public string ProfileStatus { get; set; }
        public string WerkgeverId { get; set; }
        public string WerkGeverFirstName { get; set; }
        public string WerkGeverLastName { get; set; }

    }
}

