using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using QienUrenMachien.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QienUrenMachien.Models
{
    public class RegisterViewModel 
    {
       
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password",
            ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public virtual string Firstname { get; set; }
        public virtual string Lastname { get; set; }
        public virtual string Street { get; set; }
        public virtual string City { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual string Zipcode { get; set; }
        public virtual string Country { get; set; }
        public virtual string ProfileImageUrl { get; set; }
        public virtual string BankNumber { get; set; }
        public virtual string NewProfile { get; set; }
        public string Role { get; set; }
        public string Werkgever { get; set; }
        public bool showWerkgever = false;
        public List<SelectListItem> Werkgevers{ get; set; }



    }
}
