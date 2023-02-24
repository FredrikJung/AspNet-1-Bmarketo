using Bmarketo.Models.Forms;
using Bmarketo.Services;
using Microsoft.AspNetCore.Mvc;

namespace Bmarketo.Controllers
{
    public class LoginController : Controller
    {
        private readonly AuthenticationService _authentication;

        public LoginController(AuthenticationService authentication)
        {
            _authentication = authentication;
        }

        public IActionResult Index(string returnUrl = null!)
        {
            var form = new LoginFormModel { ReturnUrl = returnUrl ?? Url.Content("~/") };
            return View(form);
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginFormModel form)
        {
            if (ModelState.IsValid)
            {
                if (await _authentication.LoginAsync(form))
                {
                    return LocalRedirect(form.ReturnUrl!);
                }
            }

            ModelState.AddModelError(string.Empty, "Incorrect email or password");
            return View(form);
        }
    }
}
