using Bmarketo.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Bmarketo.Models.ViewModels.Account
{
    public class AccountViewModel
    {
        public string? UserId { get; set; } 
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = null!;
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = null!;
        [Required]
        [Display(Name = "Street Name")]
        public string StreetName { get; set; } = null!;
        [Required]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; } = null!;
        [Required]
        [Display(Name = "City")]
        public string City { get; set; } = null!;
        [Display(Name = "Mobile")]
        public string? PhoneNumber { get; set; }
        public string? Company { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "E-mail")]
        public string Email { get; set; } = null!;
        public string? ImageName { get; set; }
        public IFormFile? ProfileImage { get; set; }
        public string ReturnUrl { get; set; } = "/";

    }
}
