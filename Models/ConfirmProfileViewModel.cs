using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QienUrenMachien.Models
{
    public class ConfirmProfileViewModel
    {
        public ConfirmProfileViewModel()
        {
            this.Profiles = new List<ProfileViewModel>();
        }
        public List<ProfileViewModel> Profiles { get; set;}
    }
}
