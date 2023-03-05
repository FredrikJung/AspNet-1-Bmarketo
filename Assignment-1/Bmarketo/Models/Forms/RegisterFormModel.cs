using System.ComponentModel.DataAnnotations;

namespace Bmarketo.Models.Forms
{
    public class RegisterFormModel
    {
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
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = null!;
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = null!;
        public IFormFile? ProfileImage { get; set; }
        public string ReturnUrl { get; set; } = null!;
        public string UserRole { get; set; } = "User";
        public bool Terms { get; set; } = false;
    }
}
