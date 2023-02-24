using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Bmarketo.Models.Forms
{
    public class LoginFormModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "E-mail")]
        public string Email { get; set; } = null!;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        public bool KeepMeLoggedIn { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
