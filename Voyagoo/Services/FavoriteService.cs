using Microsoft.EntityFrameworkCore;
using Voyagoo.Abstractions;
using Voyagoo.Contracts.Favorites;
using Voyagoo.Entities.Favorites;
using Voyagoo.Errors;
using Voyagoo.Persistence;

namespace Voyagoo.Services
{
    public class FavoriteService(VoyagooDbContext context) : IFavoriteService
    {
        private readonly VoyagooDbContext _context = context;

        public async Task<Result> AddFavoriteAsync(string userId, int? restaurantId, int? tourGuideId, int? attractionId, CancellationToken cancellationToken = default)
        {
            // لازم يبعت واحد بس
            var count = new[] { restaurantId, tourGuideId, attractionId }.Count(x => x.HasValue);
            if (count != 1)
                return Result.Failure(FavoriteErrors.InvalidFavoriteType);

            // تأكد مش موجود قبل كده
            var alreadyExists = await _context.Favorites.AnyAsync(f =>
                f.UserId == userId &&
                f.RestaurantId == restaurantId &&
                f.TourGuideId == tourGuideId &&
                f.AttractionId == attractionId,
                cancellationToken);

            if (alreadyExists)
                return Result.Failure(FavoriteErrors.AlreadyFavorited);

            var favorite = new Favorite
            {
                UserId = userId,
                RestaurantId = restaurantId,
                TourGuideId = tourGuideId,
                AttractionId = attractionId
            };

            await _context.Favorites.AddAsync(favorite, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        public async Task<Result> RemoveFavoriteAsync(string userId, int? restaurantId, int? tourGuideId, int? attractionId, CancellationToken cancellationToken = default)
        {
            var count = new[] { restaurantId, tourGuideId, attractionId }.Count(x => x.HasValue);
            if (count != 1)
                return Result.Failure(FavoriteErrors.InvalidFavoriteType);

            var favorite = await _context.Favorites.FirstOrDefaultAsync(f =>
                f.UserId == userId &&
                f.RestaurantId == restaurantId &&
                f.TourGuideId == tourGuideId &&
                f.AttractionId == attractionId,
                cancellationToken);

            if (favorite is null)
                return Result.Failure(FavoriteErrors.FavoriteNotFound);

            _context.Favorites.Remove(favorite);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        public async Task<Result<GetFavoritesResponse>> GetFavoritesAsync(string userId, CancellationToken cancellationToken = default)
        {
            var favorites = await _context.Favorites
                .Where(f => f.UserId == userId)
                .Include(f => f.Restaurant).ThenInclude(r => r!.Images)
                .Include(f => f.TourGuide)
                .Include(f => f.Attraction).ThenInclude(a => a!.Images)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var restaurants = favorites
                .Where(f => f.RestaurantId.HasValue && f.Restaurant != null && !f.Restaurant.IsDeleted)
                .Select(f => new FavoriteRestaurantItem(
                    Id: f.Restaurant!.Id,
                    Name: f.Restaurant.Name,
                    CuisineType: f.Restaurant.CuisineType.ToString(),
                    Rating: f.Restaurant.Rating,
                    MainImageUrl: f.Restaurant.Images.FirstOrDefault(i => i.IsMain)?.ImageUrl
                        ?? f.Restaurant.Images.FirstOrDefault()?.ImageUrl
                )).ToList();

            var tourGuides = favorites
                .Where(f => f.TourGuideId.HasValue && f.TourGuide != null && !f.TourGuide.IsDeleted)
                .Select(f => new FavoriteTourGuideItem(
                    Id: f.TourGuide!.Id,
                    Name: f.TourGuide.Name,
                    Rating: f.TourGuide.Rating,
                    ProfilePictureUrl: f.TourGuide.ProfilePictureUrl
                )).ToList();

            var attractions = favorites
                .Where(f => f.AttractionId.HasValue && f.Attraction != null && !f.Attraction.IsDeleted)
                .Select(f => new FavoriteAttractionItem(
                    Id: f.Attraction!.Id,
                    Name: f.Attraction.Name,
                    Place: f.Attraction.Place,
                    Rating: f.Attraction.Rating,
                    MainImageUrl: f.Attraction.Images.FirstOrDefault(i => i.IsMain)?.ImageUrl
                        ?? f.Attraction.Images.FirstOrDefault()?.ImageUrl
                )).ToList();

            return Result.Success(new GetFavoritesResponse(restaurants, tourGuides, attractions));
        }

        public async Task<Result<bool>> ToggleFavoriteAsync(string userId, int? restaurantId, int? tourGuideId, int? attractionId, CancellationToken cancellationToken = default)
        {
            var count = new[] { restaurantId, tourGuideId, attractionId }.Count(x => x.HasValue);
            if (count != 1)
                return Result.Failure<bool>(FavoriteErrors.InvalidFavoriteType);

            var existing = await _context.Favorites.FirstOrDefaultAsync(f =>
                f.UserId == userId &&
                f.RestaurantId == restaurantId &&
                f.TourGuideId == tourGuideId &&
                f.AttractionId == attractionId,
                cancellationToken);

            // لو موجود امسحه
            if (existing is not null)
            {
                _context.Favorites.Remove(existing);
                await _context.SaveChangesAsync(cancellationToken);
                return Result.Success(false); // false = اتشال
            }

            // لو مش موجود ضيفه
            var favorite = new Favorite
            {
                UserId = userId,
                RestaurantId = restaurantId,
                TourGuideId = tourGuideId,
                AttractionId = attractionId
            };

            await _context.Favorites.AddAsync(favorite, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success(true); // true = اتضاف
        }
    }
}
