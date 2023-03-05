using System.ComponentModel.DataAnnotations;

namespace Bmarketo.Models.ViewModels.Product
{
    public class ProductViewModel
    {
        public Guid Id { get; set; }
        [Required]
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
        public IFormFile? ProductImage { get; set; }
        [Required]
        public string ImageAltText { get; set; } = null!;
   
        public string? ImageSource { get; set; }
        public string? Tag { get; set; }
    }
}
