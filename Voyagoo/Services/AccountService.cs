using Microsoft.AspNetCore.Identity;
using Voyagoo.Abstractions;
using Voyagoo.Contracts.Account;
using Voyagoo.Entities;
using Voyagoo.Errors;

namespace Voyagoo.Services
{
    public class AccountService(UserManager<ApplicationUser> userManager) : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        public async Task<Result<GetProfileResponse>> GetProfileAsync(string userId, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                return Result.Failure<GetProfileResponse>(UserErrors.EmailNotFound);

            var response = new GetProfileResponse(
                
                user.FirstName,
                user.LastName,
                user.Email!,
                user.PhoneNumber
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


    }
}
