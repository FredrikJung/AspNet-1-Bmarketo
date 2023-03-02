using Bmarketo.Contexts;
using Bmarketo.Models.Entities;
using Bmarketo.Models.Forms;
using Bmarketo.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bmarketo.Services
{
    public class AuthenticationService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IdentityContext _identityContext;
        private readonly UserService _userService;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthenticationService(UserManager<IdentityUser> userManager, IdentityContext identityContext, SignInManager<IdentityUser> signInManager, UserService userService, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _identityContext = identityContext;
            _signInManager = signInManager;
            _userService = userService;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> RegisterAsync(RegisterFormModel form)
        {

            if (!await _userManager.Users.AnyAsync() || !await _roleManager.Roles.AnyAsync())
            {
                try 
                { 
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    form.UserRole = "Admin";
                } catch { }
                try { await _roleManager.CreateAsync(new IdentityRole("Product Manager")); } catch { }
                try { await _roleManager.CreateAsync(new IdentityRole("User")); } catch { }

                
            }


            if (await _identityContext.Users.AnyAsync(x => x.Email == form.Email))
            {
                return new ConflictResult();
            }

            var identityUser = new IdentityUser
            {
                UserName = form.Email,
                Email = form.Email,
                PhoneNumber = form.PhoneNumber,
                

            };

            var result = await _userManager.CreateAsync(identityUser, form.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(identityUser, form.UserRole);

                var userProfileEntity = new UserProfileEntity()
                {
                    UserId = identityUser.Id,
                    FirstName = form.FirstName,
                    LastName = form.LastName,
                    StreetName = form.StreetName,
                    PostalCode = form.PostalCode,
                    City = form.City,
                    Company = form.Company,
                    
                    
                };

                if(form.ProfileImage != null)
                {
                    userProfileEntity.ImageName = await _userService.UploadProfileImageAsync(form.ProfileImage);
                }

                _identityContext.Add(userProfileEntity);
                await _identityContext.SaveChangesAsync();

                return new OkResult();
            }

            return new BadRequestResult();

        }

        public async Task<IActionResult> LoginAsync(LoginFormModel form, bool keepMeLoggedIn = false)
        {
            if(await _identityContext.Users.AnyAsync(x => x.Email == form.Email))
            {
                var result = await _signInManager.PasswordSignInAsync(form.Email, form.Password, keepMeLoggedIn, false);
                if(result.Succeeded)
                {
                    return new OkResult();
                }
            }

            return new BadRequestResult();
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }


}
