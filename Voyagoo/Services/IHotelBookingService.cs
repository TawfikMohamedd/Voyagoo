using Voyagoo.Abstractions;
using Voyagoo.Contracts.Hotels;

namespace Voyagoo.Services
{
    public interface IHotelBookingService
    {
        Task<Result<CreateHotelBookingResponse>> CreateBookingAsync(
            int hotelId,
            string userId,
            CreateHotelBookingRequest request,
            CancellationToken cancellationToken = default);
    }
}