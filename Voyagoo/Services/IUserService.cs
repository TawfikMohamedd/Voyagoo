using Voyagoo.Abstractions;
using Voyagoo.Contracts.Users;

namespace Voyagoo.Services
{
    public interface IUserService
    {
        Task<Result<GetUsersAdminResponse>> GetAllUsersAdminAsync(CancellationToken cancellationToken = default);
        Task<Result<bool>> ToggleUserStatusAsync(string userId, CancellationToken cancellationToken = default);
    }
}
