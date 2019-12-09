using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace QienUrenMachien.Models
{
    public class ProfileViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Street { get; set; }
        public string Zipcode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string BankNumber { get; set; }
        public string ProfileImageUrl { get; set; }
        public IFormFile ProfileImage { get; set; }
    }
}