using Mapster;
using Microsoft.EntityFrameworkCore;
using Voyagoo.Abstractions;
using Voyagoo.Contracts.TourGuides;
using Voyagoo.Entities.TourGuides;
using Voyagoo.Errors;
using Voyagoo.Persistence;

namespace Voyagoo.Services
{
    public class TourGuideService(
        VoyagooDbContext context,
        IImageService imageService) : ITourGuideService
    {
        private readonly VoyagooDbContext _context = context;
        private readonly IImageService _imageService = imageService;

        // ─────────────────────────────────────────────
        // PUBLIC
        // ─────────────────────────────────────────────

        public async Task<Result<List<GetTourGuidesResponse>>> GetAllTourGuidesAsync(CancellationToken cancellationToken = default)
        {
            var guides = await _context.TourGuides
                .Where(g => !g.IsDeleted && g.Status == TourGuideStatus.Active)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            return Result.Success(guides.Adapt<List<GetTourGuidesResponse>>());
        }

        public async Task<Result<GetTourGuideDetailsResponse>> GetTourGuideByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var guide = await _context.TourGuides
                .Where(g => g.Id == id && !g.IsDeleted && g.Status == TourGuideStatus.Active)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            if (guide is null)
                return Result.Failure<GetTourGuideDetailsResponse>(TourGuideErrors.TourGuideNotFound);

            return Result.Success(guide.Adapt<GetTourGuideDetailsResponse>());
        }

        // ─────────────────────────────────────────────
        // ADMIN
        // ─────────────────────────────────────────────

        public async Task<Result<GetTourGuideDetailsResponse>> AddTourGuideAsync(AddTourGuideRequest request, CancellationToken cancellationToken = default)
        {
            var isDuplicate = await _context.TourGuides
                .AnyAsync(g => g.Email == request.Email && !g.IsDeleted, cancellationToken);

            if (isDuplicate)
                return Result.Failure<GetTourGuideDetailsResponse>(TourGuideErrors.DuplicateEmail);

            var guide = new TourGuide
            {
                Name = request.Name,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Description = request.Description,
                Rating = request.Rating,
                PricePerDay = request.PricePerDay,
                Languages = request.Languages.Select(l => (Language)l).ToList()
            };

            await _context.TourGuides.AddAsync(guide, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success(guide.Adapt<GetTourGuideDetailsResponse>());
        }

        public async Task<Result> AddTourGuideImageAsync(int id, IFormFile image, CancellationToken cancellationToken = default)
        {
            var guide = await _context.TourGuides
                .FirstOrDefaultAsync(g => g.Id == id && !g.IsDeleted, cancellationToken);

            if (guide is null)
                return Result.Failure(TourGuideErrors.TourGuideNotFound);

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var extension = Path.GetExtension(image.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
                return Result.Failure(TourGuideErrors.InvalidImageFile);

            // حذف الصورة القديمة من Cloudinary لو موجودة
            if (!string.IsNullOrEmpty(guide.ProfilePictureUrl))
                await _imageService.DeleteImageAsync(guide.ProfilePictureUrl);

            // رفع الصورة الجديدة على Cloudinary
            var imageUrl = await _imageService.UploadImageAsync(image, "voyagoo/tourguides", cancellationToken);

            guide.ProfilePictureUrl = imageUrl;
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        public async Task<Result> DeleteTourGuideAsync(int id, CancellationToken cancellationToken = default)
        {
            var guide = await _context.TourGuides
                .FirstOrDefaultAsync(g => g.Id == id && !g.IsDeleted, cancellationToken);

            if (guide is null)
                return Result.Failure(TourGuideErrors.TourGuideNotFound);

            guide.IsDeleted = true;
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        public async Task<Result<GetTourGuideDetailsResponse>> UpdateTourGuideAsync(int id, UpdateTourGuideRequest request, CancellationToken cancellationToken = default)
        {
            var guide = await _context.TourGuides
                .FirstOrDefaultAsync(g => g.Id == id && !g.IsDeleted, cancellationToken);

            if (guide is null)
                return Result.Failure<GetTourGuideDetailsResponse>(TourGuideErrors.TourGuideNotFound);

            var isDuplicate = await _context.TourGuides
                .AnyAsync(g => g.Email == request.Email && g.Id != id && !g.IsDeleted, cancellationToken);

            if (isDuplicate)
                return Result.Failure<GetTourGuideDetailsResponse>(TourGuideErrors.DuplicateEmail);

            guide.Name = request.Name;
            guide.Email = request.Email;
            guide.PhoneNumber = request.PhoneNumber;
            guide.Description = request.Description;
            guide.Rating = request.Rating;
            guide.PricePerDay = request.PricePerDay;
            guide.Languages = request.Languages.Select(l => (Language)l).ToList();

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success(guide.Adapt<GetTourGuideDetailsResponse>());
        }

        public async Task<Result> UpdateTourGuideStatusAsync(int id, TourGuideStatus status, CancellationToken cancellationToken = default)
        {
            var guide = await _context.TourGuides
                .FirstOrDefaultAsync(g => g.Id == id && !g.IsDeleted, cancellationToken);

            if (guide is null)
                return Result.Failure(TourGuideErrors.TourGuideNotFound);

            guide.Status = status;
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        public async Task<Result<GetTourGuidesAdminResponse>> GetAllTourGuidesAdminAsync(CancellationToken cancellationToken = default)
        {
            var guides = await _context.TourGuides
                .Where(g => !g.IsDeleted)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var items = guides.Select(g =>
            {
                var languageNames = g.Languages.Select(l => l.ToString()).ToList();

                var languagesDisplay = languageNames.Count <= 2
                    ? string.Join(", ", languageNames)
                    : string.Join(", ", languageNames.Take(2)) + $" +{languageNames.Count - 2}";

                return new TourGuideAdminItem(
                    Id: g.Id,
                    Name: g.Name,
                    Email: g.Email,
                    PhoneNumber: g.PhoneNumber,
                    Languages: languagesDisplay,
                    Rating: g.Rating,
                    Status: g.Status.ToString(),
                    ProfilePictureUrl: g.ProfilePictureUrl
                );
            }).ToList();

            var response = new GetTourGuidesAdminResponse(
                TotalTourGuides: guides.Count,
                ActiveTourGuides: guides.Count(g => g.Status == TourGuideStatus.Active),
                InactiveTourGuides: guides.Count(g => g.Status == TourGuideStatus.Inactive),
                TourGuides: items
            );

            return Result.Success(response);
        }

        public IEnumerable<object> GetAllLanguages()
        {
            return Enum.GetValues<Language>()
                .Select(l => new { id = (int)l, name = l.ToString() });
        }
    }
}
