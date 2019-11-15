using System.ComponentModel.DataAnnotations;

namespace QienUrenMachien.Models
{
    public class CreateRoleViewModel
    {
        [Required]
        public string RoleName {get; set;}

    }
}