using Microsoft.AspNetCore.Identity;
using Voyagoo.Abstractions;
using Voyagoo.Contracts.Account;
using Voyagoo.Entities;
using Voyagoo.Errors;

namespace Voyagoo.Services
{
    public class AccountService(UserManager<ApplicationUser> userManager,
    IWebHostEnvironment webHostEnvironment) : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

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

            // التحقق من نوع الملف
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var extension = Path.GetExtension(image.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
                return Result.Failure<string>(UserErrors.InvalidImageFile);

            // حذف الصورة القديمة لو موجودة
            if (!string.IsNullOrEmpty(user.ProfilePictureUrl))
            {
                var oldPath = Path.Combine(_webHostEnvironment.WebRootPath, user.ProfilePictureUrl.TrimStart('/'));
                if (File.Exists(oldPath))
                    File.Delete(oldPath);
            }

            // حفظ الصورة الجديدة
            var fileName = $"{Guid.NewGuid()}{extension}";
            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "users");

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await image.CopyToAsync(stream, cancellationToken);

            // تحديث الـ URL في الـ DB
            user.ProfilePictureUrl = $"/images/users/{fileName}";

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

