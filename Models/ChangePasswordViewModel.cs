using Microsoft.AspNetCore.Identity;
using QienUrenMachien.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QienUrenMachien.Models
{
    public class ChangePasswordViewModel 
    {
        [Required]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        public string Email { get; set; }


    }
}
