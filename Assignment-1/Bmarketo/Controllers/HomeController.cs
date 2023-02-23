using Microsoft.AspNetCore.Mvc;

namespace Bmarketo.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
