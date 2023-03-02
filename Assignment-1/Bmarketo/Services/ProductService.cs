using Bmarketo.Contexts;
using Bmarketo.Models.Entities;
using Bmarketo.Models.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bmarketo.Services
{
    public class ProductService
    {
        private readonly DataContext _context;
        private readonly UserService _userService;

        public ProductService(DataContext context, UserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<IActionResult> CreateProductAsync(ProductFormModel form)
        {

            if (await _context.Products.AnyAsync(x => x.Name == form.Name))
            {
                return new ConflictResult();
            }

            
            if (form != null)
            {         
                var productEntity = new ProductEntity()
                {
                    Name = form.Name,
                    ShortDescription = form.ShortDescription,
                    LongDescription = form.LongDescription,
                    Price = form.Price,
                    DiscountPrice = form.DiscountPrice,
                    Category = form.Category,
                    ImageAltText = form.ImageAltText,
                    Tag = form.Tag,

                };

                if (form.ProductImage != null)
                {
                    productEntity.ImageSource = await _userService.UploadProfileImageAsync(form.ProductImage);
                }

                _context.Add(productEntity);
                await _context.SaveChangesAsync();

                return new OkResult();
            }

            return new BadRequestResult();

        }

        public async Task<List<ProductEntity>> GetAllProductsAsync()
        {
            try
            {
                var products = await _context.Products.ToListAsync();
                return products;
            }
            catch { return null!; }
        }
    }
}
