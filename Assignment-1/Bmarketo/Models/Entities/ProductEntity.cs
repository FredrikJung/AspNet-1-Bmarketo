using System.ComponentModel.DataAnnotations;

namespace Bmarketo.Models.Entities
{
    public class ProductEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
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
        [Required]
        public string ImageSource { get; set; } = null!;
        [Required]
        public string ImageAltText { get; set; } = null!;
        public string? Tag { get; set; }
                                                       
    }
}
