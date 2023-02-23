using Bmarketo.Contexts;
using Bmarketo.Models.Forms;
using Bmarketo.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace Bmarketo.Services
{
    public class AuthenticationService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IdentityContext _identityContext;

        public AuthenticationService(UserManager<IdentityUser> userManager, IdentityContext identityContext, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _identityContext = identityContext;
            _signInManager = signInManager;
        }

        public async Task<bool> RegisterAsync(RegisterForm form)
        {
            var identityUser = new IdentityUser
            {
                UserName = form.Email,
                Email = form.Email,
                PhoneNumber = form.PhoneNumber,

            };

            var identityProfile = new IdentityUserProfile
            {
                UserId = identityUser.Id,
                FirstName = form.FirstName,
                LastName = form.LastName,
                StreetName = form.StreetName,
                PostalCode = form.PostalCode,
                City = form.City,
                Company = form.Company
            };

            var result = await _userManager.CreateAsync(identityUser, form.Password);
            if(result.Succeeded)
            {
                _identityContext.UserProfiles.Add(identityProfile);
                await _identityContext.SaveChangesAsync();

                return true;
            }


            return false;
        }

        public async Task<bool> LoginAsync(LoginForm form, bool keepMeLoggedIn = false)
        {
            var result = await _signInManager.PasswordSignInAsync(form.Email, form.Password, keepMeLoggedIn, false);
            return result.Succeeded;
        }
    }


}
