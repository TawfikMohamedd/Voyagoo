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
        IWebHostEnvironment webHostEnvironment) : IRestaurantService
    {
        private readonly VoyagooDbContext _context = context;
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

        // ─────────────────────────────────────────────
        // PUBLIC
        // ─────────────────────────────────────────────

        public async Task<Result<List<GetRestaurantsResponse>>> GetAllRestaurantsAsync(CancellationToken cancellationToken = default)
        {
            var restaurants = await _context.Restaurants
                .Where(r => !r.IsDeleted)
                .Include(r => r.Images)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var response = restaurants.Adapt<List<GetRestaurantsResponse>>();

            return Result.Success(response);
        }

        public async Task<Result<GetRestaurantDetailsResponse>> GetRestaurantByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var restaurant = await _context.Restaurants
                .Where(r => r.Id == id && !r.IsDeleted)
                .Include(r => r.Images)
                .Include(r => r.Features)
                    .ThenInclude(f => f.Feature)
                .Include(r => r.Comments)
                    .ThenInclude(c => c.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            if (restaurant is null)
                return Result.Failure<GetRestaurantDetailsResponse>(RestaurantErrors.RestaurantNotFound);

            var response = restaurant.Adapt<GetRestaurantDetailsResponse>();

            return Result.Success(response);
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
            // تأكد إن الـ FeatureIds موجودة في الـ DB
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

            // Reload مع الـ Features عشان نرجع Response كامل
            var created = await _context.Restaurants
                .Where(r => r.Id == restaurant.Id)
                .Include(r => r.Images)
                .Include(r => r.Features)
                    .ThenInclude(f => f.Feature)
                .Include(r => r.Comments)
                    .ThenInclude(c => c.User)
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

            // تحديد لو في main image موجودة خلاص
            var hasMainImage = restaurant.Images.Any(i => i.IsMain);

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };

            foreach (var image in images)
            {
                var extension = Path.GetExtension(image.FileName).ToLower();

                if (!allowedExtensions.Contains(extension))
                    return Result.Failure(RestaurantErrors.InvalidImageFile);

                // حفظ الصورة لوكال
                var fileName = $"{Guid.NewGuid()}{extension}";
                var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "restaurants");

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var filePath = Path.Combine(folderPath, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await image.CopyToAsync(stream, cancellationToken);

                // أول صورة تتحط تبقى main لو مفيش main
                var isMain = !hasMainImage;
                hasMainImage = true;

                restaurant.Images.Add(new RestaurantImage
                {
                    ImageUrl = $"/images/restaurants/{fileName}",
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

            // Soft Delete
            restaurant.IsDeleted = true;

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        public async Task<Result<GetRestaurantDetailsResponse>> UpdateRestaurantAsync(int id,UpdateRestaurantRequest request,CancellationToken cancellationToken = default)
        {
            var restaurant = await _context.Restaurants
                .Include(r => r.Features)
                .Include(r => r.Images)
                .Include(r => r.Comments)
                    .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted, cancellationToken);

            if (restaurant is null)
                return Result.Failure<GetRestaurantDetailsResponse>(RestaurantErrors.RestaurantNotFound);

            // تأكد إن الـ FeatureIds موجودة في الـ DB
            var featuresExist = await _context.Features
                .Where(f => request.FeatureIds.Contains(f.Id))
                .CountAsync(cancellationToken);

            if (featuresExist != request.FeatureIds.Count)
                return Result.Failure<GetRestaurantDetailsResponse>(RestaurantErrors.FeatureNotFound);

            // update البيانات الأساسية
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

            // update الـ Features
            restaurant.Features = request.FeatureIds.Select(fId => new RestaurantFeature
            {
                RestaurantId = id,
                FeatureId = fId
            }).ToList();

            await _context.SaveChangesAsync(cancellationToken);

            // Reload عشان نرجع الـ Features محملة
            var updated = await _context.Restaurants
                .Where(r => r.Id == id)
                .Include(r => r.Images)
                .Include(r => r.Features)
                    .ThenInclude(f => f.Feature)
                .Include(r => r.Comments)
                    .ThenInclude(c => c.User)
                .AsNoTracking()
                .FirstAsync(cancellationToken);

            return Result.Success(updated.Adapt<GetRestaurantDetailsResponse>());
        }

        // ─────────────────────────────────────────────
        // ADMIN - FEATURES
        // ─────────────────────────────────────────────

        public async Task<Result<List<FeatureResponse>>> GetAllFeaturesAsync(CancellationToken cancellationToken = default)
        {
            var features = await _context.Features
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var response = features.Adapt<List<FeatureResponse>>();

            return Result.Success(response);
        }

        public async Task<Result<FeatureResponse>> AddFeatureAsync(AddFeatureRequest request, CancellationToken cancellationToken = default)
        {
            // تأكد مفيش feature بنفس الاسم
            var isDuplicate = await _context.Features
                .AnyAsync(f => f.Name == request.Name, cancellationToken);

            if (isDuplicate)
                return Result.Failure<FeatureResponse>(RestaurantErrors.DuplicateFeature);

            var feature = request.Adapt<Feature>();

            await _context.Features.AddAsync(feature, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success(feature.Adapt<FeatureResponse>());
        }
    }
}
