using Bmarketo.Contexts;
using Bmarketo.Models.Entities;
using Bmarketo.Models.Identity;
using Bmarketo.Models.ViewModels.Account;
using Bmarketo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Bmarketo.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly UserService _userService;
        private readonly IdentityContext _identityContext;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<IdentityUser> userManager, UserService userService, IdentityContext identityContext, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _userService = userService;
            _identityContext = identityContext;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {

            var users = await _userManager.Users.ToListAsync();

            return View(users);
        }

        public async Task<IActionResult> EditUser(string id, string ReturnUrl)
        {
            var viewModel = new AccountViewModel { ReturnUrl = ReturnUrl ?? Url.Content("/") };
            viewModel = await _userService.GetUserAccountAdminAsync(id);

            return View(viewModel);
            
        }
        [HttpPost]
        public async Task<IActionResult> EditUser(AccountViewModel viewModel, string id)
        {
            

            var user = await _userService.GetUserAccountAdminAsync(id);
            viewModel.UserId = user.UserId;
            

            if (ModelState.IsValid)
            {
                var result = await _userService.UpdateUserAsync(viewModel);
                if (result is OkResult)
                {
                    return RedirectToAction("Index", "Admin");
                }

                else
                {
                    ModelState.AddModelError(string.Empty, "An unexpected error occured. Please contact us!");
                }
            }

            return View(viewModel);

        }

        public async Task<IActionResult> ManageUserRoles(string userId)
        {
            ViewBag.UserId = userId;

            var user = await _userManager.FindByIdAsync(userId);

            if(user == null)
            {
                ModelState.AddModelError(string.Empty, $"User with Id: {userId} cannot be found");
            }

            var viewModelList = new List<UserRolesViewModel>();

            foreach(var role in _roleManager.Roles.ToList())
            {
                var viewModel = new UserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name!
                };

                if(await _userManager.IsInRoleAsync(user!, role.Name!))
                {
                    viewModel.IsSelected = true;
                }

                else
                {
                    viewModel.IsSelected = false;
                }
                viewModelList.Add(viewModel);
            }

            return View(viewModelList);
        }

        [HttpPost]
        public async Task<IActionResult> ManageUserRoles(List<UserRolesViewModel> viewModel, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if(user == null)
            {
                ModelState.AddModelError(string.Empty, $"User with Id: {userId} cannot be found");
            }

            var roles = await _userManager.GetRolesAsync(user!);
            var result = await _userManager.RemoveFromRolesAsync(user!, roles);

            if(!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Cannot remove user existing roles");
            }

            result = await _userManager.AddToRolesAsync(user!,
                viewModel.Where(x => x.IsSelected).Select(y => y.RoleName));

            if(!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Cannot add selected roles to user");
            }

            return RedirectToAction("Index", "Admin" );
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if(user == null)
            {
                ModelState.AddModelError(string.Empty, $"User with Id: {id} cannot be found");
                return RedirectToAction("Index", "Admin");
            }

            else
            {
               var result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Admin");
                }

                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return RedirectToAction("Index", "Admin");
            }
        }
    }
}

