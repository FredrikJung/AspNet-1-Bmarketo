using Bmarketo.Contexts;
using Bmarketo.Models.Entities;
using Bmarketo.Models.Forms;
using Bmarketo.Models.ViewModels.Product;
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

        public async Task<ProductEntity> GetProductAsync(string id)
        {
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(x => x.Id.ToString() == id);
                return product!;
            }
            catch { return null!; }

        }

        public async Task<ProductViewModel> GetProductDataAsync(string id)
        {
            var productEntity = await _context.Products.FirstOrDefaultAsync(x => x.Id.ToString() == id);

            if(productEntity != null)
            {
                var product = new ProductViewModel
                {
                    Id = productEntity.Id,
                    Name = productEntity.Name,
                    ShortDescription = productEntity.ShortDescription,
                    LongDescription = productEntity.LongDescription,
                    Price = productEntity.Price,
                    DiscountPrice = productEntity.DiscountPrice,
                    ImageAltText = productEntity.ImageAltText,
                    ImageSource = productEntity.ImageSource,
                    Category = productEntity.Category,
                    Tag = productEntity.Tag,
                };
                return product;
            }
            return null!;
        }

        public async Task<IActionResult> UpdateProductAsync (ProductViewModel product)
        {
            var productEntity = await _context.Products.FirstOrDefaultAsync(x => x.Id == product.Id);
            if(productEntity != null)
            {
                productEntity.Name = product.Name;
                productEntity.ShortDescription = product.ShortDescription;
                productEntity.LongDescription = product.LongDescription;
                productEntity.Price = product.Price;
                productEntity.DiscountPrice = product.DiscountPrice;
                productEntity.Category = product.Category;
                productEntity.ImageAltText = product.ImageAltText;
                productEntity.Tag = product.Tag;
               

                if(product.ProductImage != null)
                {
                    productEntity.ImageSource = await _userService.UploadProfileImageAsync(product.ProductImage);
                }
                _context.Entry(productEntity).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return new OkResult();
            }

            return new BadRequestResult();
        }
    }
}
