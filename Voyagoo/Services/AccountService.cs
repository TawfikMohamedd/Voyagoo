using Microsoft.AspNetCore.Identity;
using Voyagoo.Abstractions;
using Voyagoo.Contracts.Account;
using Voyagoo.Entities;
using Voyagoo.Errors;

namespace Voyagoo.Services
{
    public class AccountService(
        UserManager<ApplicationUser> userManager,
        IImageService imageService) : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IImageService _imageService = imageService;

        public async Task<Result<GetProfileResponse>> GetProfileAsync(string userId, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                return Result.Failure<GetProfileResponse>(UserErrors.EmailNotFound);

            var response = new GetProfileResponse(
                user.FirstName,
                user.LastName,
                user.Email!,
                user.PhoneNumber,
                user.ProfilePictureUrl
            );

            return Result.Success(response);
        }

        public async Task<Result> UpdateProfileAsync(string userId, UpdateProfileRequest request, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                return Result.Failure(UserErrors.EmailNotFound);

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.PhoneNumber = request.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var error = result.Errors.First();
                return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
            }

            return Result.Success();
        }

        public async Task<Result<string>> UpdateProfilePictureAsync(string userId, IFormFile image, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                return Result.Failure<string>(UserErrors.EmailNotFound);

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var extension = Path.GetExtension(image.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
                return Result.Failure<string>(UserErrors.InvalidImageFile);

            // حذف الصورة القديمة من Cloudinary لو موجودة
            if (!string.IsNullOrEmpty(user.ProfilePictureUrl))
                await _imageService.DeleteImageAsync(user.ProfilePictureUrl);

            // رفع الصورة الجديدة على Cloudinary
            var imageUrl = await _imageService.UploadImageAsync(image, "voyagoo/users", cancellationToken);

            user.ProfilePictureUrl = imageUrl;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var error = result.Errors.First();
                return Result.Failure<string>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
            }

            return Result.Success(user.ProfilePictureUrl);
        }
    }


}

