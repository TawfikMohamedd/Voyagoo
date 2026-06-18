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

        Task<Result> ConfirmBookingAsync(
            int bookingId,
            string userId,
            ConfirmHotelBookingRequest request,
            CancellationToken cancellationToken = default);

        Task<Result<GetBookingHistoryResponse>> GetBookingHistoryAsync(
            string userId,
            CancellationToken cancellationToken = default);
    }
}