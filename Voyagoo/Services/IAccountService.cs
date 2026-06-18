using Voyagoo.Abstractions;
using Voyagoo.Contracts.Account;

namespace Voyagoo.Services
{
    public interface IAccountService
    {
        Task<Result<GetProfileResponse>> GetProfileAsync(string userId, CancellationToken cancellationToken = default);
        Task<Result> UpdateProfileAsync(string userId, UpdateProfileRequest request, CancellationToken cancellationToken = default);
        Task<Result<string>> UpdateProfilePictureAsync(string userId, IFormFile image, CancellationToken cancellationToken = default);
        Task<Result<GetAllBookingsResponse>> GetAllBookingsAsync(string userId,CancellationToken cancellationToken = default);
        Task<Result> DeleteBookingAsync(string userId,int bookingId,string bookingType,CancellationToken cancellationToken = default);
    }
}
