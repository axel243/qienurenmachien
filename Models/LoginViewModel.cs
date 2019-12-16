using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QienUrenMachien.Models
{
    public class LoginViewModel
    {
        [Required (ErrorMessage = "Voer een email in.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required (ErrorMessage = "Voer je wachtwoord in.")]
        [DataType(DataType.Password, ErrorMessage = "Onjuist wachtwoord.")]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}
