using Voyagoo.Abstractions;
using Voyagoo.Contracts.TourGuides;
using Voyagoo.Entities.TourGuides;

namespace Voyagoo.Services
{
    public interface ITourGuideService
    {
        
        Task<Result<List<GetTourGuidesResponse>>> GetAllTourGuidesAsync(CancellationToken cancellationToken = default);
        Task<Result<GetTourGuideDetailsResponse>> GetTourGuideByIdAsync(int id, CancellationToken cancellationToken = default);

        
        Task<Result<GetTourGuideDetailsResponse>> AddTourGuideAsync(AddTourGuideRequest request, CancellationToken cancellationToken = default);
        Task<Result> AddTourGuideImageAsync(int id, IFormFile image, CancellationToken cancellationToken = default);
        Task<Result> DeleteTourGuideAsync(int id, CancellationToken cancellationToken = default);
        Task<Result<GetTourGuideDetailsResponse>> UpdateTourGuideAsync(int id, UpdateTourGuideRequest request, CancellationToken cancellationToken = default);
        Task<Result> UpdateTourGuideStatusAsync(int id, TourGuideStatus status, CancellationToken cancellationToken = default);
        Task<Result<GetTourGuidesAdminResponse>> GetAllTourGuidesAdminAsync(CancellationToken cancellationToken = default);
        Task<Result<GetTourGuideDetailsResponse>> GetTourGuideByIdAdminAsync(int id, CancellationToken cancellationToken = default);


        // Helper للـ Frontend
        IEnumerable<object> GetAllLanguages();
    }
}
