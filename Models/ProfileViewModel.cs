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
        [Phone]
        [StringLength(13)]
        public string PhoneNumber { get; set; }
        [StringLength(30)]
        public string Street { get; set; }
        [StringLength(8)]
        public string Zipcode { get; set; }
        [StringLength(30)]
        public string City { get; set; }
        [StringLength(30)]
        public string Country { get; set; }
        [StringLength(30)]
        public string BankNumber { get; set; }
        public string ProfileImageUrl { get; set; }
        public IFormFile ProfileImage { get; set; }
    }
}