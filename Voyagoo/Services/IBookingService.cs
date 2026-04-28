using Voyagoo.Abstractions;
using Voyagoo.Contracts.Restaurants;

namespace Voyagoo.Services
{
    public interface IBookingService
    {
        Task<Result<CreateBookingResponse>> CreateBookingAsync(
            int restaurantId,
            string userId,
            CreateBookingRequest request,
            CancellationToken cancellationToken = default);
    }
}
