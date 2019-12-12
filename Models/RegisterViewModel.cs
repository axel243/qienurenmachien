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

        //[Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password",
            ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(30)]
        public virtual string Firstname { get; set; }
        [Required]
        public virtual string Lastname { get; set; }
        [StringLength(40)]
        public virtual string Street { get; set; }
        [StringLength(40)]
        public virtual string City { get; set; }
        [Phone]
        public virtual string PhoneNumber { get; set; }
        [StringLength(8)]
        public virtual string Zipcode { get; set; }
        [StringLength(40)]
        public virtual string Country { get; set; }
        public virtual string ProfileImageUrl { get; set; }
        public virtual string BankNumber { get; set; }
        public virtual string NewProfile { get; set; }
        public virtual string Iban { get; set; }

        [Display(Name = "Actief vanaf")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ActiveFrom{ get; set; }
        public string Role { get; set; }
        public string Werkgever { get; set; }
        public List<SelectListItem> Werkgevers{ get; set; }



    }
}
