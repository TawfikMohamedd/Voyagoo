using Voyagoo.Abstractions;
using Voyagoo.Contracts.Restaurants;

namespace Voyagoo.Services
{
    public interface IRestaurantService
    {
        // Public endpoints
        Task<Result<List<GetRestaurantsResponse>>> GetAllRestaurantsAsync(CancellationToken cancellationToken = default);
        Task<Result<GetRestaurantDetailsResponse>> GetRestaurantByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Result> AddCommentAsync(int restaurantId, string userId, AddCommentRequest request, CancellationToken cancellationToken = default);

        // Admin endpoints - Restaurants
        Task<Result<GetRestaurantDetailsResponse>> AddRestaurantAsync(AddRestaurantRequest request, CancellationToken cancellationToken = default);
        Task<Result> AddRestaurantImagesAsync(int restaurantId, List<IFormFile> images, CancellationToken cancellationToken = default);
        Task<Result> DeleteRestaurantAsync(int id, CancellationToken cancellationToken = default);

        // Admin endpoints - Features
        Task<Result<List<FeatureResponse>>> GetAllFeaturesAsync(CancellationToken cancellationToken = default);
        Task<Result<FeatureResponse>> AddFeatureAsync(AddFeatureRequest request, CancellationToken cancellationToken = default);
    }
}
