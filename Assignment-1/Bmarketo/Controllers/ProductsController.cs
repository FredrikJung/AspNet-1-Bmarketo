using Bmarketo.Contexts;
using Bmarketo.Models.Entities;
using Bmarketo.Models.Forms;
using Bmarketo.Models.ViewModels.Product;
using Bmarketo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bmarketo.Controllers
{
    public class ProductsController : Controller
    {

        private readonly ProductService _productService;
        private readonly DataContext _context;

        public ProductsController(ProductService productService, DataContext context)
        {
            _productService = productService;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllProductsAsync();
            return View(products);
        }

        public async Task<IActionResult> ProductDetails(string id)
        {
            var product = await _productService.GetProductAsync(id);
            var products = await _productService.GetAllProductsAsync();
            var tuple = new Tuple<IEnumerable<ProductEntity>, ProductEntity>(products, product);

            return View(tuple);
        }


        [Authorize(Roles = "Admin, Product Manager")]
        public IActionResult CreateProducts(string ReturnUrl = null!)
        {
            var form = new ProductFormModel { ReturnUrl = ReturnUrl ?? Url.Content("/") };
            return View(form);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProducts(ProductFormModel form)
        {
            if (ModelState.IsValid)
            {
                var result = await _productService.CreateProductAsync(form);
                if (result is OkResult)
                {
                    return LocalRedirect(form.ReturnUrl);
                }
                else if (result is ConflictResult)
                {
                    ModelState.AddModelError(string.Empty, "Product with this name already exists");
                }
                else if(result is NoContentResult)
                {
                    ModelState.AddModelError(string.Empty, "A product needs an image. Add an image and try again!");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "An unexpected error occured. Please try again!");
                }
            }

            return View(form);
        }

        [Authorize(Roles = "Admin, Product Manager")]
        public async Task<IActionResult> EditProduct(string id)
        {
            var product = await _productService.GetProductDataAsync(id);
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(ProductViewModel product)
        {
            if (ModelState.IsValid)
            {
                var result = await _productService.UpdateProductAsync(product);
                if (result is OkResult)
                {
                    return RedirectToAction("Index", "Products");
                }

                else if (result is BadRequestResult) 
                {
                    ModelState.AddModelError(string.Empty, "An unexpected error occured. Please try again!");
                }
            }

            return View(product);
            
        }

        [Authorize(Roles = "Admin, Product Manager")]
        [HttpPost]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id.ToString() == id);
            

            if (product == null)
            {
                ModelState.AddModelError(string.Empty, $"Product with Id: {id} cannot be found");
                return RedirectToAction("Index", "Products");
            }

            else
            {
                var result =  _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Products");

            }
        }

    }
}
