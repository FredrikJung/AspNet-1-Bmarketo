using Bmarketo.Models.Forms;
using Microsoft.AspNetCore.Mvc;

namespace Bmarketo.Controllers
{
    public class ContactsController : Controller
    {
        public IActionResult Index(string returnUrl = null!)
        {
            var form = new ContactFormModel { ReturnUrl = returnUrl ?? Url.Content("~/") };
            return View(form);
        }    

        [HttpPost]
        public IActionResult Index(ContactFormModel form)
        {
            return View();
        }
    }
}
