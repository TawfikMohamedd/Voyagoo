using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Voyagoo.Abstractions;
using Voyagoo.Abstractions.Consts;
using Voyagoo.Contracts.Users;
using Voyagoo.Entities;

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
                Users: users.Select(u => new UserAdminItem(
                    Id: u.Id,
                    FirstName: u.FirstName,
                    LastName: u.LastName,
                    Email: u.Email!,
                    PhoneNumber: u.PhoneNumber
                )).ToList()
            );

            return Result.Success(response);
        }
    }
}