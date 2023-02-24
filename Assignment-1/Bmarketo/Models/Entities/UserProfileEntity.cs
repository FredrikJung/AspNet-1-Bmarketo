using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Bmarketo.Models.Entities
{
    public class UserProfileEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string UserId { get; set; } = null!;
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
        public string? Company { get; set; }
        public string? ImageName { get; set; }
        public IdentityUser User { get; set; } = null!; 
    }
}
