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
        private readonly IWebHostEnvironment _env;

        public UserService(UserManager<IdentityUser> usermanager, IdentityContext identityContext, IWebHostEnvironment env)
        {
            _Usermanager = usermanager;
            _identityContext = identityContext;
            _env = env;
        }

        public async Task<UserProfileModel> GetUserAccountAsync(string username)
        {
            var identityUser = await _Usermanager.Users.FirstOrDefaultAsync(x => x.UserName == username);
            if (identityUser != null)
            {
                var identityProfile = await _identityContext.UserProfiles.FirstOrDefaultAsync(x => x.UserId == identityUser.Id);

                if (identityProfile != null)
                {
                    var userProfile = new UserProfileModel
                    {
                        Id = identityUser.Id,
                        FirstName = identityProfile.FirstName,
                        LastName = identityProfile.LastName,
                        Email = identityUser.Email!,
                        PhoneNumber = identityUser.PhoneNumber,
                        StreetName = identityProfile.StreetName,
                        PostalCode = identityProfile.PostalCode,
                        City = identityProfile.City,
                        Company = identityProfile.Company,
                        ImageName = identityProfile.ImageName
                        
                    };

                    return userProfile;
                }
            }
            return null!;
        }
        public async Task<string> UploadProfileImageAsync(IFormFile profileImage)
        {
            var profilePath = $"{_env.WebRootPath}/images/profiles";
            var imageName = $"profile_{Guid.NewGuid()}{Path.GetExtension(profileImage.FileName)}";
            string filePath = $"{profilePath}/{imageName}";

            using var fs = new FileStream(filePath, FileMode.Create);
            await profileImage.CopyToAsync(fs);

            return imageName;
        }
    }
}
