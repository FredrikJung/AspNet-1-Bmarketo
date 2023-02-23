using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Bmarketo.Models.Forms
{
    public class ContactForm
    {
        [Required]
        [Display(Name = "Your Name")]
        public string Name { get; set; } = null!;

        [Required]
        [EmailAddress]
        [Display(Name = "Your E-mail")]
        public string Email { get; set; } = null!;

        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }
        public string? Company { get; set; }

        [Required]
        [Display(Name = "Something write")]
        public string SomethingWrite { get; set; } = null!;
        public string? ReturnUrl { get; set; } 
    }
}
