using Voyagoo.Abstractions;
using Voyagoo.Contracts.TourGuides;

namespace Voyagoo.Services
{
    public interface ITourGuideBookingService
    {
        Task<Result<CreateTourGuideBookingResponse>> CreateBookingAsync(
            int tourGuideId,
            string userId,
            CreateTourGuideBookingRequest request,
            CancellationToken cancellationToken = default);

        Task<Result> ConfirmBookingAsync(
            int bookingId,
            string userId,
            ConfirmTourGuideBookingRequest request,
            CancellationToken cancellationToken = default);

        Task<Result<GetTourGuideBookingHistoryResponse>> GetBookingHistoryAsync(
            string userId,
            CancellationToken cancellationToken = default);
    }
}
