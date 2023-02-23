using Bmarketo.Models.Forms;
using Microsoft.AspNetCore.Mvc;

namespace Bmarketo.Controllers
{
    public class ContactsController : Controller
    {
        public IActionResult Index(string returnUrl = null!)
        {
            var form = new ContactForm { ReturnUrl = returnUrl ?? Url.Content("~/") };
            return View(form);
        }    

        [HttpPost]
        public IActionResult Index(ContactForm form)
        {
            return View();
        }
    }
}
