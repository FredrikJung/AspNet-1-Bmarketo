using Bmarketo.Contexts;
using Bmarketo.Models.Entities;
using Bmarketo.Models.Forms;
using Bmarketo.Models.Identity;
using Bmarketo.Models.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bmarketo.Services
{
    public class UserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IdentityContext _identityContext;
        private readonly IWebHostEnvironment _env;

        public UserService(UserManager<IdentityUser> userManager, IdentityContext identityContext, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _identityContext = identityContext;
            _env = env;
        }

        public async Task<AccountViewModel> GetUserAccountAsync(string username)
        {
            var identityUser = await _identityContext.Users.FirstOrDefaultAsync(x => x.UserName == username);
            if (identityUser != null)
            {
                var identityProfile = await _identityContext.UserProfiles.FirstOrDefaultAsync(x => x.UserId == identityUser.Id);

                if (identityProfile != null)
                {
                    var viewModel = new AccountViewModel
                    {
                        UserId = identityUser.Id,
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

                    return viewModel;
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

        public async Task<IActionResult> UpdateUserAsync(AccountViewModel viewModel)
        {

            //var userProfileEntity = await _identityContext.UserProfiles.FindAsync(viewModel.UserId);
            var userProfileEntity = await _identityContext.UserProfiles.FirstOrDefaultAsync(x => x.UserId == viewModel.UserId);
            var identityUser = await _identityContext.Users.FirstOrDefaultAsync(x => x.UserName == viewModel.Email);


            if (identityUser?.Email != viewModel.Email)
            {
                return new ConflictResult();
            }

            if (identityUser != null)
            {

                identityUser.PhoneNumber= viewModel.PhoneNumber;

                if (userProfileEntity != null)
                {
                    userProfileEntity.FirstName = viewModel.FirstName;
                    userProfileEntity.LastName = viewModel.LastName;
                    userProfileEntity.StreetName = viewModel.StreetName;
                    userProfileEntity.PostalCode = viewModel.PostalCode;
                    userProfileEntity.City = viewModel.City;
                    userProfileEntity.Company = viewModel.Company;

                    if (viewModel.ProfileImage != null)
                    {
                        userProfileEntity.ImageName = await UploadProfileImageAsync(viewModel.ProfileImage);
                    }

                    _identityContext.Entry(userProfileEntity).State = EntityState.Modified;
                    await _identityContext.SaveChangesAsync();


                    return new OkResult();
                }
            }

            return new BadRequestResult();
        }
    }
}
