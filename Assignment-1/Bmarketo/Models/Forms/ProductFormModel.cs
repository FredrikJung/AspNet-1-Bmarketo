using System.ComponentModel.DataAnnotations;

namespace Bmarketo.Models.Forms
{
    public class ProductFormModel
    {
        public string Name { get; set; } = null!;
        [Required]
        public string ShortDescription { get; set; } = null!;
        [Required]
        public string LongDescription { get; set; } = null!;
        [Required]
        public decimal Price { get; set; }
        public decimal DiscountPrice { get; set; }
        [Required]
        public string Category { get; set; } = null!;

        [Required]
        public string ImageAltText { get; set; } = null!;
        public string? Tag { get; set; }
        public IFormFile? ProductImage { get; set; }
        public string ReturnUrl { get; set; } = null!;
    }
}
