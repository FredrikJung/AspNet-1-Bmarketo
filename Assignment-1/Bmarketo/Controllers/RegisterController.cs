using Bmarketo.Models.Forms;
using Bmarketo.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bmarketo.Controllers
{
    public class RegisterController : Controller
    {
        private readonly AuthenticationService _authentication;
        private readonly UserManager<IdentityUser> _userManager;

        public RegisterController(AuthenticationService authentication, UserManager<IdentityUser> userManager)
        {
            _authentication = authentication;
            _userManager = userManager;
        }

        public IActionResult Index(string returnUrl = null!)
        {
            var form = new RegisterForm { ReturnUrl = returnUrl ?? Url.Content("~/") };
            return View(form);

        }

        [HttpPost]
        public async Task<IActionResult> Index(RegisterForm form)
        {
            if(ModelState.IsValid)
            {
                if(await _userManager.Users.AnyAsync(x => x.Email == form.Email))
                {
                    ModelState.AddModelError(string.Empty, "An account with this email already exists.");
                    return View(form);
                }

                if (await _authentication.RegisterAsync(form))
                {
                    return LocalRedirect(form.ReturnUrl!);
                }
                else
                {
                    return View(form);
                }
            }

            return View(form);

        }
    }
}
