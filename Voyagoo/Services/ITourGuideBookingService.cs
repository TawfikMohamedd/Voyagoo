using Voyagoo.Abstractions;
using Voyagoo.Contracts.TourGuides;

namespace Voyagoo.Services
{
    public interface ITourGuideBookingService
    {
        Task<Result<CreateTourGuideBookingResponse>> CreateBookingAsync(int tourGuideId,string userId,CreateTourGuideBookingRequest request, CancellationToken cancellationToken = default);
    }
}
