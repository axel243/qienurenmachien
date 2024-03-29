using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using QienUrenMachien.Entities;

namespace QienUrenMachien.Models
{
    public class ApplicationUserViewModel : IdentityUser
    {
        public virtual List<TimeSheet> TimeSheet { get; set; }
        public virtual string Firstname { get; set; }
        public virtual string Lastname { get; set; }
        public virtual string Street { get; set; }
        public virtual string City { get; set; }
        public virtual string Zipcode { get; set; }
        public virtual string Country { get; set; }
        public virtual string ProfileImageUrl { get; set; }
        public virtual string BankNumber { get; set; }
        public virtual string NewProfile { get; set; }
    }
}
