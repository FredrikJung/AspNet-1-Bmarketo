using Bmarketo.Contexts;
using Bmarketo.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bmarketo.Services
{
    public class UserService
    {
        private readonly UserManager<IdentityUser> _Usermanager;
        private readonly IdentityContext _identityContext;

        public UserService(UserManager<IdentityUser> usermanager, IdentityContext identityContext)
        {
            _Usermanager = usermanager;
            _identityContext = identityContext;
        }

        public async Task<UserAccount> GetUserAccountAsync(string id)
        {
            var identityUser = await _Usermanager.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (identityUser != null)
            {
                var identityProfile = await _identityContext.UserProfiles.FirstOrDefaultAsync(x => x.UserId == identityUser.Id);

                if (identityProfile != null)
                {
                    return new UserAccount
                    {
                        Id = identityUser.Id,
                        FirstName = identityProfile.FirstName,
                        LastName = identityProfile.LastName,
                        Email = identityUser.Email!,
                        PhoneNumber = identityUser.PhoneNumber,
                        StreetName = identityProfile.StreetName,
                        PostalCode = identityProfile.PostalCode,
                        City = identityProfile.City,
                        Company = identityProfile.Company
                    };
                }
            }
            return null!;
        }
    }
}
