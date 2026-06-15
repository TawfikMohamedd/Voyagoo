using Mapster;
using Microsoft.EntityFrameworkCore;
using Voyagoo.Abstractions;
using Voyagoo.Contracts.Hotels;
using Voyagoo.Entities.Hotels;
using Voyagoo.Errors;
using Voyagoo.Persistence;

namespace Voyagoo.Services
{
    public class HotelService(
        VoyagooDbContext context,
        IImageService imageService) : IHotelService
    {
        private readonly VoyagooDbContext _context = context;
        private readonly IImageService _imageService = imageService;

        // ─────────────────────────────────────────────
        // PUBLIC
        // ─────────────────────────────────────────────

        public async Task<Result<List<GetHotelsResponse>>> GetAllHotelsAsync(CancellationToken cancellationToken = default)
        {
            var hotels = await _context.Hotels
                .Where(h => !h.IsDeleted && h.Status == HotelStatus.Active)
                .Include(h => h.Images)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return Result.Success(hotels.Adapt<List<GetHotelsResponse>>());
        }

        public async Task<Result<GetHotelDetailsResponse>> GetHotelByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var hotel = await _context.Hotels
                .Where(h => h.Id == id && !h.IsDeleted && h.Status == HotelStatus.Active)
                .Include(h => h.Images)
                .Include(h => h.Features).ThenInclude(f => f.HotelFeature)
                .Include(h => h.Comments).ThenInclude(c => c.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            if (hotel is null)
                return Result.Failure<GetHotelDetailsResponse>(HotelErrors.HotelNotFound);

            return Result.Success(hotel.Adapt<GetHotelDetailsResponse>());
        }

        // ─────────────────────────────────────────────
        // ADMIN - HOTELS
        // ─────────────────────────────────────────────

        public async Task<Result<GetHotelDetailsResponse>> AddHotelAsync(AddHotelRequest request, CancellationToken cancellationToken = default)
        {
            var featuresExist = await _context.HotelFeatures
                .Where(f => request.FeatureIds.Contains(f.Id))
                .CountAsync(cancellationToken);

            if (featuresExist != request.FeatureIds.Count)
                return Result.Failure<GetHotelDetailsResponse>(HotelErrors.FeatureNotFound);

            var hotel = request.Adapt<Hotel>();

            hotel.Features = request.FeatureIds.Select(fId => new HotelFeatureMap
            {
                HotelFeatureId = fId
            }).ToList();

            await _context.Hotels.AddAsync(hotel, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var created = await _context.Hotels
                .Where(h => h.Id == hotel.Id)
                .Include(h => h.Images)
                .Include(h => h.Features).ThenInclude(f => f.HotelFeature)
                .AsNoTracking()
                .FirstAsync(cancellationToken);

            return Result.Success(created.Adapt<GetHotelDetailsResponse>());
        }

        public async Task<Result> AddHotelImagesAsync(int hotelId, List<IFormFile> images, CancellationToken cancellationToken = default)
        {
            var hotel = await _context.Hotels
                .Include(h => h.Images)
                .FirstOrDefaultAsync(h => h.Id == hotelId && !h.IsDeleted, cancellationToken);

            if (hotel is null)
                return Result.Failure(HotelErrors.HotelNotFound);

            var hasMainImage = hotel.Images.Any(i => i.IsMain);
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };

            foreach (var image in images)
            {
                var extension = Path.GetExtension(image.FileName).ToLower();

                if (!allowedExtensions.Contains(extension))
                    return Result.Failure(HotelErrors.InvalidImageFile);

                var imageUrl = await _imageService.UploadImageAsync(image, "voyagoo/hotels", cancellationToken);

                var isMain = !hasMainImage;
                hasMainImage = true;

                hotel.Images.Add(new HotelImage
                {
                    ImageUrl = imageUrl,
                    IsMain = isMain
                });
            }

            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        public async Task<Result> DeleteHotelAsync(int id, CancellationToken cancellationToken = default)
        {
            var hotel = await _context.Hotels
                .FirstOrDefaultAsync(h => h.Id == id && !h.IsDeleted, cancellationToken);

            if (hotel is null)
                return Result.Failure(HotelErrors.HotelNotFound);

            hotel.IsDeleted = true;
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        public async Task<Result<GetHotelDetailsResponse>> UpdateHotelAsync(int id, UpdateHotelRequest request, CancellationToken cancellationToken = default)
        {
            var hotel = await _context.Hotels
                .Include(h => h.Features)
                .Include(h => h.Images)
                .FirstOrDefaultAsync(h => h.Id == id && !h.IsDeleted, cancellationToken);

            if (hotel is null)
                return Result.Failure<GetHotelDetailsResponse>(HotelErrors.HotelNotFound);

            var featuresExist = await _context.HotelFeatures
                .Where(f => request.FeatureIds.Contains(f.Id))
                .CountAsync(cancellationToken);

            if (featuresExist != request.FeatureIds.Count)
                return Result.Failure<GetHotelDetailsResponse>(HotelErrors.FeatureNotFound);

            hotel.Name = request.Name;
            hotel.Description = request.Description;
            hotel.Location = request.Location;
            hotel.Rating = request.Rating;
            hotel.SingleRooms = request.SingleRooms;
            hotel.SinglePrice = request.SinglePrice;
            hotel.DoubleRooms = request.DoubleRooms;
            hotel.DoublePrice = request.DoublePrice;
            hotel.TripleRooms = request.TripleRooms;
            hotel.TriplePrice = request.TriplePrice;
            hotel.SuiteRooms = request.SuiteRooms;
            hotel.SuitePrice = request.SuitePrice;
            hotel.Discount = request.Discount;
            hotel.ServiceCharge = request.ServiceCharge;

            hotel.Features = request.FeatureIds.Select(fId => new HotelFeatureMap
            {
                HotelId = id,
                HotelFeatureId = fId
            }).ToList();

            await _context.SaveChangesAsync(cancellationToken);

            var updated = await _context.Hotels
                .Where(h => h.Id == id)
                .Include(h => h.Images)
                .Include(h => h.Features).ThenInclude(f => f.HotelFeature)
                .AsNoTracking()
                .FirstAsync(cancellationToken);

            return Result.Success(updated.Adapt<GetHotelDetailsResponse>());
        }

        public async Task<Result> DeleteHotelImageAsync(int hotelId, int imageId, CancellationToken cancellationToken = default)
        {
            var hotel = await _context.Hotels
                .Include(h => h.Images)
                .FirstOrDefaultAsync(h => h.Id == hotelId && !h.IsDeleted, cancellationToken);

            if (hotel is null)
                return Result.Failure(HotelErrors.HotelNotFound);

            var image = hotel.Images.FirstOrDefault(i => i.Id == imageId);
            if (image is null)
                return Result.Failure(HotelErrors.ImageNotFound);

            await _imageService.DeleteImageAsync(image.ImageUrl);

            hotel.Images.Remove(image);

            if (image.IsMain && hotel.Images.Count > 0)
                hotel.Images.First().IsMain = true;

            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        public async Task<Result> UpdateHotelStatusAsync(int id, HotelStatus status, CancellationToken cancellationToken = default)
        {
            var hotel = await _context.Hotels
                .FirstOrDefaultAsync(h => h.Id == id && !h.IsDeleted, cancellationToken);

            if (hotel is null)
                return Result.Failure(HotelErrors.HotelNotFound);

            hotel.Status = status;
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        public async Task<Result<GetHotelsAdminResponse>> GetAllHotelsAdminAsync(CancellationToken cancellationToken = default)
        {
            var hotels = await _context.Hotels
                .Where(h => !h.IsDeleted)
                .Include(h => h.Images)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var response = new GetHotelsAdminResponse(
                TotalHotels: hotels.Count,
                ActiveHotels: hotels.Count(h => h.Status == HotelStatus.Active),
                InactiveHotels: hotels.Count(h => h.Status == HotelStatus.Inactive),
                Hotels: hotels.Adapt<List<HotelAdminItem>>()
            );

            return Result.Success(response);
        }

        public async Task<Result<GetHotelDetailsResponse>> GetHotelByIdAdminAsync(int id, CancellationToken cancellationToken = default)
        {
            var hotel = await _context.Hotels
                .Where(h => h.Id == id && !h.IsDeleted)
                .Include(h => h.Images)
                .Include(h => h.Features).ThenInclude(f => f.HotelFeature)
                .Include(h => h.Comments).ThenInclude(c => c.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            if (hotel is null)
                return Result.Failure<GetHotelDetailsResponse>(HotelErrors.HotelNotFound);

            return Result.Success(hotel.Adapt<GetHotelDetailsResponse>());
        }

        // ─────────────────────────────────────────────
        // ADMIN - FEATURES
        // ─────────────────────────────────────────────

        public async Task<Result<List<HotelFeatureResponse>>> GetAllHotelFeaturesAsync(CancellationToken cancellationToken = default)
        {
            var features = await _context.HotelFeatures
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return Result.Success(features.Adapt<List<HotelFeatureResponse>>());
        }

        public async Task<Result<HotelFeatureResponse>> AddHotelFeatureAsync(AddHotelFeatureRequest request, CancellationToken cancellationToken = default)
        {
            var isDuplicate = await _context.HotelFeatures
                .AnyAsync(f => f.Name == request.Name, cancellationToken);

            if (isDuplicate)
                return Result.Failure<HotelFeatureResponse>(HotelErrors.DuplicateFeature);

            var feature = request.Adapt<HotelFeature>();

            await _context.HotelFeatures.AddAsync(feature, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success(feature.Adapt<HotelFeatureResponse>());
        }

        public async Task<Result> AddCommentAsync(int hotelId, string userId, AddHotelCommentRequest request, CancellationToken cancellationToken = default)
        {
            var hotelExists = await _context.Hotels
                .AnyAsync(h => h.Id == hotelId && !h.IsDeleted, cancellationToken);

            if (!hotelExists)
                return Result.Failure(HotelErrors.HotelNotFound);

            var comment = new HotelComment
            {
                Content = request.Content,
                Rating = request.Rating,
                HotelId = hotelId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            await _context.HotelComments.AddAsync(comment, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        public async Task<Result<GetHotelCommentsResponse>> GetHotelCommentsAsync(int hotelId, CancellationToken cancellationToken = default)
        {
            var hotel = await _context.Hotels
                .FirstOrDefaultAsync(h => h.Id == hotelId && !h.IsDeleted, cancellationToken);

            if (hotel is null)
                return Result.Failure<GetHotelCommentsResponse>(HotelErrors.HotelNotFound);

            var comments = await _context.HotelComments
                .Where(c => c.HotelId == hotelId)
                .Include(c => c.User)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var response = new GetHotelCommentsResponse(
                TotalComments: comments.Count,
                AverageRating: comments.Count > 0 ? Math.Round(comments.Average(c => c.Rating), 1) : 0,
                Comments: comments.Select(c => new HotelCommentResponse(
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

        public async Task<Result> DeleteCommentAsync(int hotelId, int commentId, CancellationToken cancellationToken = default)
        {
            var comment = await _context.HotelComments
                .FirstOrDefaultAsync(c => c.Id == commentId && c.HotelId == hotelId, cancellationToken);

            if (comment is null)
                return Result.Failure(HotelErrors.CommentNotFound);

            _context.HotelComments.Remove(comment);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        public async Task<Result> DeleteOwnCommentAsync(int hotelId, int commentId, string userId, CancellationToken cancellationToken = default)
        {
            var comment = await _context.HotelComments
                .FirstOrDefaultAsync(c => c.Id == commentId && c.HotelId == hotelId, cancellationToken);

            if (comment is null)
                return Result.Failure(HotelErrors.CommentNotFound);

            if (comment.UserId != userId)
                return Result.Failure(HotelErrors.CommentNotOwned);

            _context.HotelComments.Remove(comment);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
