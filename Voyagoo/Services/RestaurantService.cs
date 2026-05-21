using Mapster;
using Microsoft.EntityFrameworkCore;
using Voyagoo.Abstractions;
using Voyagoo.Contracts.Restaurants;
using Voyagoo.Entities.Restaurants;
using Voyagoo.Errors;
using Voyagoo.Persistence;

namespace Voyagoo.Services
{
    public class RestaurantService(
        VoyagooDbContext context,
        IImageService imageService) : IRestaurantService
    {
        private readonly VoyagooDbContext _context = context;
        private readonly IImageService _imageService = imageService;

        // ─────────────────────────────────────────────
        // PUBLIC
        // ─────────────────────────────────────────────

        public async Task<Result<List<GetRestaurantsResponse>>> GetAllRestaurantsAsync(CancellationToken cancellationToken = default)
        {
            var restaurants = await _context.Restaurants
                .Where(r => !r.IsDeleted && r.Status == RestaurantStatus.Active)
                .Include(r => r.Images)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return Result.Success(restaurants.Adapt<List<GetRestaurantsResponse>>());
        }

        public async Task<Result<GetRestaurantDetailsResponse>> GetRestaurantByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var restaurant = await _context.Restaurants
                .Where(r => r.Id == id && !r.IsDeleted && r.Status == RestaurantStatus.Active)
                .Include(r => r.Images)
                .Include(r => r.Features).ThenInclude(f => f.Feature)
                .Include(r => r.Comments).ThenInclude(c => c.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            if (restaurant is null)
                return Result.Failure<GetRestaurantDetailsResponse>(RestaurantErrors.RestaurantNotFound);

            return Result.Success(restaurant.Adapt<GetRestaurantDetailsResponse>());
        }

        public async Task<Result> AddCommentAsync(int restaurantId, string userId, AddCommentRequest request, CancellationToken cancellationToken = default)
        {
            var restaurantExists = await _context.Restaurants
                .AnyAsync(r => r.Id == restaurantId && !r.IsDeleted, cancellationToken);

            if (!restaurantExists)
                return Result.Failure(RestaurantErrors.RestaurantNotFound);

            var comment = new RestaurantComment
            {
                Content = request.Content,
                Rating = request.Rating,
                RestaurantId = restaurantId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            await _context.RestaurantComments.AddAsync(comment, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        // ─────────────────────────────────────────────
        // ADMIN - RESTAURANTS
        // ─────────────────────────────────────────────

        public async Task<Result<GetRestaurantDetailsResponse>> AddRestaurantAsync(AddRestaurantRequest request, CancellationToken cancellationToken = default)
        {
            var featuresExist = await _context.Features
                .Where(f => request.FeatureIds.Contains(f.Id))
                .CountAsync(cancellationToken);

            if (featuresExist != request.FeatureIds.Count)
                return Result.Failure<GetRestaurantDetailsResponse>(RestaurantErrors.FeatureNotFound);

            var restaurant = request.Adapt<Restaurant>();

            restaurant.Features = request.FeatureIds.Select(fId => new RestaurantFeature
            {
                FeatureId = fId
            }).ToList();

            await _context.Restaurants.AddAsync(restaurant, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var created = await _context.Restaurants
                .Where(r => r.Id == restaurant.Id)
                .Include(r => r.Images)
                .Include(r => r.Features).ThenInclude(f => f.Feature)
                .Include(r => r.Comments).ThenInclude(c => c.User)
                .AsNoTracking()
                .FirstAsync(cancellationToken);

            return Result.Success(created.Adapt<GetRestaurantDetailsResponse>());
        }

        public async Task<Result> AddRestaurantImagesAsync(int restaurantId, List<IFormFile> images, CancellationToken cancellationToken = default)
        {
            var restaurant = await _context.Restaurants
                .Include(r => r.Images)
                .FirstOrDefaultAsync(r => r.Id == restaurantId && !r.IsDeleted, cancellationToken);

            if (restaurant is null)
                return Result.Failure(RestaurantErrors.RestaurantNotFound);

            var hasMainImage = restaurant.Images.Any(i => i.IsMain);
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };

            foreach (var image in images)
            {
                var extension = Path.GetExtension(image.FileName).ToLower();

                if (!allowedExtensions.Contains(extension))
                    return Result.Failure(RestaurantErrors.InvalidImageFile);

                // رفع على Cloudinary
                var imageUrl = await _imageService.UploadImageAsync(image, "voyagoo/restaurants", cancellationToken);

                var isMain = !hasMainImage;
                hasMainImage = true;

                restaurant.Images.Add(new RestaurantImage
                {
                    ImageUrl = imageUrl,
                    IsMain = isMain
                });
            }

            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        public async Task<Result> DeleteRestaurantAsync(int id, CancellationToken cancellationToken = default)
        {
            var restaurant = await _context.Restaurants
                .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted, cancellationToken);

            if (restaurant is null)
                return Result.Failure(RestaurantErrors.RestaurantNotFound);

            restaurant.IsDeleted = true;
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        public async Task<Result<GetRestaurantDetailsResponse>> UpdateRestaurantAsync(int id, UpdateRestaurantRequest request, CancellationToken cancellationToken = default)
        {
            var restaurant = await _context.Restaurants
                .Include(r => r.Features)
                .Include(r => r.Images)
                .Include(r => r.Comments).ThenInclude(c => c.User)
                .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted, cancellationToken);

            if (restaurant is null)
                return Result.Failure<GetRestaurantDetailsResponse>(RestaurantErrors.RestaurantNotFound);

            var featuresExist = await _context.Features
                .Where(f => request.FeatureIds.Contains(f.Id))
                .CountAsync(cancellationToken);

            if (featuresExist != request.FeatureIds.Count)
                return Result.Failure<GetRestaurantDetailsResponse>(RestaurantErrors.FeatureNotFound);

            restaurant.Name = request.Name;
            restaurant.Description = request.Description;
            restaurant.Address = request.Address;
            restaurant.Rating = request.Rating;
            restaurant.CuisineType = request.CuisineType;
            restaurant.MinPrice = request.MinPrice;
            restaurant.MaxPrice = request.MaxPrice;
            restaurant.TablesForTwo = request.TablesForTwo;
            restaurant.TablesForFour = request.TablesForFour;
            restaurant.TablesForSix = request.TablesForSix;

            restaurant.Features = request.FeatureIds.Select(fId => new RestaurantFeature
            {
                RestaurantId = id,
                FeatureId = fId
            }).ToList();

            await _context.SaveChangesAsync(cancellationToken);

            var updated = await _context.Restaurants
                .Where(r => r.Id == id)
                .Include(r => r.Images)
                .Include(r => r.Features).ThenInclude(f => f.Feature)
                .Include(r => r.Comments).ThenInclude(c => c.User)
                .AsNoTracking()
                .FirstAsync(cancellationToken);

            return Result.Success(updated.Adapt<GetRestaurantDetailsResponse>());
        }

        public async Task<Result> DeleteRestaurantImageAsync(int restaurantId, int imageId, CancellationToken cancellationToken = default)
        {
            var restaurant = await _context.Restaurants
                .Include(r => r.Images)
                .FirstOrDefaultAsync(r => r.Id == restaurantId && !r.IsDeleted, cancellationToken);

            if (restaurant is null)
                return Result.Failure(RestaurantErrors.RestaurantNotFound);

            var image = restaurant.Images.FirstOrDefault(i => i.Id == imageId);
            if (image is null)
                return Result.Failure(RestaurantErrors.ImageNotFound);

            // حذف من Cloudinary
            await _imageService.DeleteImageAsync(image.ImageUrl);

            restaurant.Images.Remove(image);

            if (image.IsMain && restaurant.Images.Count > 0)
                restaurant.Images.First().IsMain = true;

            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        public async Task<Result> UpdateRestaurantStatusAsync(int id, RestaurantStatus status, CancellationToken cancellationToken = default)
        {
            var restaurant = await _context.Restaurants
                .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted, cancellationToken);

            if (restaurant is null)
                return Result.Failure(RestaurantErrors.RestaurantNotFound);

            restaurant.Status = status;
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        public async Task<Result<GetRestaurantsAdminResponse>> GetAllRestaurantsAdminAsync(CancellationToken cancellationToken = default)
        {
            var restaurants = await _context.Restaurants
                .Where(r => !r.IsDeleted)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var response = new GetRestaurantsAdminResponse(
                TotalRestaurants: restaurants.Count,
                ActiveRestaurants: restaurants.Count(r => r.Status == RestaurantStatus.Active),
                InactiveRestaurants: restaurants.Count(r => r.Status == RestaurantStatus.Inactive),
                Restaurants: restaurants.Adapt<List<RestaurantAdminItem>>()
            );

            return Result.Success(response);
        }

        // ─────────────────────────────────────────────
        // ADMIN - FEATURES
        // ─────────────────────────────────────────────

        public async Task<Result<List<FeatureResponse>>> GetAllFeaturesAsync(CancellationToken cancellationToken = default)
        {
            var features = await _context.Features
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return Result.Success(features.Adapt<List<FeatureResponse>>());
        }

        public async Task<Result<FeatureResponse>> AddFeatureAsync(AddFeatureRequest request, CancellationToken cancellationToken = default)
        {
            var isDuplicate = await _context.Features
                .AnyAsync(f => f.Name == request.Name, cancellationToken);

            if (isDuplicate)
                return Result.Failure<FeatureResponse>(RestaurantErrors.DuplicateFeature);

            var feature = request.Adapt<Feature>();

            await _context.Features.AddAsync(feature, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success(feature.Adapt<FeatureResponse>());
        }

        public async Task<Result<GetRestaurantCommentsResponse>> GetRestaurantCommentsAsync(int restaurantId, CancellationToken cancellationToken = default)
        {
            var restaurant = await _context.Restaurants
                .FirstOrDefaultAsync(r => r.Id == restaurantId && !r.IsDeleted, cancellationToken);

            if (restaurant is null)
                return Result.Failure<GetRestaurantCommentsResponse>(RestaurantErrors.RestaurantNotFound);

            var comments = await _context.RestaurantComments
                .Where(c => c.RestaurantId == restaurantId)
                .Include(c => c.User)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var response = new GetRestaurantCommentsResponse(
                TotalComments: comments.Count,
                AverageRating: comments.Count > 0 ? Math.Round(comments.Average(c => c.Rating), 1) : 0,
                Comments: comments.Select(c => new CommentResponse(
                    c.Id,
                    c.User.FirstName + " " + c.User.LastName,
                    c.User.ProfilePictureUrl,
                    c.Content,
                    c.Rating,
                    DateOnly.FromDateTime(c.CreatedAt)
                )).ToList()
            );

            return Result.Success(response);
        }
        public async Task<Result> DeleteCommentAsync(int restaurantId, int commentId, CancellationToken cancellationToken = default)
        {
            var comment = await _context.RestaurantComments
                .FirstOrDefaultAsync(c => c.Id == commentId && c.RestaurantId == restaurantId, cancellationToken);

            if (comment is null)
                return Result.Failure(RestaurantErrors.CommentNotFound);

            _context.RestaurantComments.Remove(comment);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
