using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace QienUrenMachien.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual List<TimeSheet> TimeSheet { get; set; }
    }
}
