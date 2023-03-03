using System.ComponentModel.DataAnnotations;

namespace Bmarketo.Models.ViewModels.Home
{
    public class IndexHomeViewModel
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }                             
        public string? ShortDescription { get; set; }
        [Required]
        public string ImageSource { get; set; } = null!;
        [Required]
        public string ImageAltText { get; set; } = null!;

        public string? Tag { get; set; }

    }
}
