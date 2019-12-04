using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace QienUrenMachien.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public virtual List<TimeSheet> TimeSheet { get; set; }
        public virtual List<ActivityLog> ActivityLogs { get; set; }
        public virtual string Firstname { get; set; }
        public virtual string Lastname { get; set; }
        public virtual string Street { get; set; }
        public virtual string City { get; set; }
        public virtual string Zipcode { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual string Country { get; set; }
        public virtual string ProfileImageUrl { get; set; }
        public virtual string BankNumber { get; set; }
        public virtual string NewProfile { get; set; }
        public virtual string WerkgeverID { get; set; }
  
    }
}
