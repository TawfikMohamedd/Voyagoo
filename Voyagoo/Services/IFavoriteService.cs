using Voyagoo.Abstractions;
using Voyagoo.Contracts.Favorites;

namespace Voyagoo.Services
{
    public interface IFavoriteService
    {
        Task<Result<bool>> ToggleFavoriteAsync(string userId, int? restaurantId, int? tourGuideId, int? attractionId, int? hotelId, CancellationToken cancellationToken = default);
        Task<Result<GetFavoritesResponse>> GetFavoritesAsync(string userId, CancellationToken cancellationToken = default);
    }
}
