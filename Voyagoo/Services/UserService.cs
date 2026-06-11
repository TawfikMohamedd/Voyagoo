using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Voyagoo.Abstractions;
using Voyagoo.Abstractions.Consts;
using Voyagoo.Contracts.Users;
using Voyagoo.Entities;
using Voyagoo.Errors;

namespace Voyagoo.Services
{
    public class UserService(UserManager<ApplicationUser> userManager) : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        public async Task<Result<GetUsersAdminResponse>> GetAllUsersAdminAsync(CancellationToken cancellationToken = default)
        {
            var membersIds = (await _userManager.GetUsersInRoleAsync(DefaultRoles.Member))
                .Select(u => u.Id)
                .ToHashSet();

            var users = await _userManager.Users
                .Where(u => membersIds.Contains(u.Id))
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var response = new GetUsersAdminResponse(
                TotalUsers: users.Count,
                ActiveUsers: users.Count(u => u.IsActive),
                InactiveUsers: users.Count(u => !u.IsActive),
                Users: users.Select(u => new UserAdminItem(
                    Id: u.Id,
                    FirstName: u.FirstName,
                    LastName: u.LastName,
                    Email: u.Email!,
                    PhoneNumber: u.PhoneNumber,
                    IsActive: u.IsActive
                )).ToList()
            );

            return Result.Success(response);
        }


        public async Task<Result<bool>> ToggleUserStatusAsync(string userId, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                return Result.Failure<bool>(UserErrors.EmailNotFound);

            user.IsActive = !user.IsActive;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var error = result.Errors.First();
                return Result.Failure<bool>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
            }

            return Result.Success(user.IsActive);
        }

    }
}