using Voyagoo.Abstractions;
using Voyagoo.Contracts.TourGuides;

namespace Voyagoo.Services
{
    public interface ITourGuideService
    {
        
        Task<Result<List<GetTourGuidesResponse>>> GetAllTourGuidesAsync(CancellationToken cancellationToken = default);
        Task<Result<GetTourGuideDetailsResponse>> GetTourGuideByIdAsync(int id, CancellationToken cancellationToken = default);

        
        Task<Result<GetTourGuideDetailsResponse>> AddTourGuideAsync(AddTourGuideRequest request, CancellationToken cancellationToken = default);
        Task<Result> AddTourGuideImageAsync(int id, IFormFile image, CancellationToken cancellationToken = default);
        Task<Result> DeleteTourGuideAsync(int id, CancellationToken cancellationToken = default);

        // Helper للـ Frontend
        IEnumerable<object> GetAllLanguages();
    }
}
