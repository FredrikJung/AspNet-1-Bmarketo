using Bmarketo.Models.Entities;
using Bmarketo.Models.Forms;
using Bmarketo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bmarketo.Controllers
{
    public class ProductsController : Controller
    {

        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
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


        [Authorize(Roles = "Admin, Project Manager")]
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
                else
                {
                    ModelState.AddModelError(string.Empty, "An unexpected error occured. Please try again!");
                }
            }

            return View(form);
        }

    }
}
