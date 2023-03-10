using System.ComponentModel.DataAnnotations;

namespace Bmarketo.Models.Identity
{
    public class UserProfileModel
    {
        public string UserId { get; set; } = null!;

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = null!;
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = null!;
        [Required]
        [EmailAddress]
        [Display(Name = "E-mail")]
        public string Email { get; set; } = null!;
        [Display(Name = "Mobile")]
        public string? PhoneNumber { get; set; }
        [Required]
        [Display(Name = "Street Name")]
        public string StreetName { get; set; } = null!;
        [Required]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; } = null!;
        [Required]
        [Display(Name = "City")]
        public string City { get; set; } = null!;
        public string? Company { get; set; }
        public string? ImageName { get; set; }
        

    }
}
