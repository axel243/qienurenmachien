using QienUrenMachien.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QienUrenMachien.Models
{
    public class UserViewModel
    {
        public ApplicationUser theUser { get; set; }

        [Display(Name = "Actief tot")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ActiveUntilParam { get; set; }
    }
}
