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

        public RegisterController(AuthenticationService authentication)
        {
            _authentication = authentication;
        }

        public IActionResult Index(string ReturnUrl = null!)
        {
            var form = new RegisterFormModel { ReturnUrl = ReturnUrl ?? Url.Content("/") };
            return View(form);

        }

        [HttpPost]
        public async Task<IActionResult> Index(RegisterFormModel form)
        {
            if(form.Terms != false)
            {
                if (ModelState.IsValid)
                {
                    var result = await _authentication.RegisterAsync(form);
                    if (result is OkResult)
                    {
                        return LocalRedirect(form.ReturnUrl);
                    }
                    else if (result is ConflictResult)
                    {
                        ModelState.AddModelError(string.Empty, "User with this email already exists");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "An unexpected error occured. Please try again!");
                    }
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Please accept the terms and service to sign up!");
            }
        
             return View(form);
        }
    }
}
