using Bmarketo.Models.Forms;
using Bmarketo.Models.Identity;
using Bmarketo.Models.ViewModels.Account;
using Bmarketo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bmarketo.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserService _userService;
        private readonly AuthenticationService _authentication;
        private readonly UserManager<IdentityUser> _userManager;

        public AccountController(UserService userService, AuthenticationService authentication, UserManager<IdentityUser> userManager)
        {
            _userService = userService;
            _authentication = authentication;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string ReturnUrl = null!)
        {
            var viewModel = new AccountViewModel { ReturnUrl = ReturnUrl ?? Url.Content("/") };
            viewModel = await _userService.GetUserAccountAsync(User.Identity!.Name!);

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(AccountViewModel viewModel)
        {
            var user = await _userService.GetUserAccountAsync(User.Identity!.Name!);
            viewModel.UserId = user.UserId;
            
            
            if (ModelState.IsValid)
            {
                var result = await _userService.UpdateUserAsync(viewModel);
                if (result is OkResult)
                {
                    return LocalRedirect(viewModel.ReturnUrl);
                }

                else if(result is ConflictResult)
                {
                    ModelState.AddModelError(string.Empty, $"You can not change your email. Reset your email to: {user.Email} and try again!");
                }

                else
                {
                    ModelState.AddModelError(string.Empty, "An unexpected error occured. Please contact us!");
                }
            }

            return View(viewModel);

        }

        public async Task<IActionResult> Logout()
        {
            await _authentication.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}

   





