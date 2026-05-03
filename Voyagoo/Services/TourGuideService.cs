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
        IWebHostEnvironment webHostEnvironment) : ITourGuideService
    {
        private readonly VoyagooDbContext _context = context;
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

        // ─────────────────────────────────────────────
        // PUBLIC
        // ─────────────────────────────────────────────

        public async Task<Result<List<GetTourGuidesResponse>>> GetAllTourGuidesAsync(CancellationToken cancellationToken = default)
        {
            var guides = await _context.TourGuides
                .Where(g => !g.IsDeleted)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var response = guides.Adapt<List<GetTourGuidesResponse>>();
            return Result.Success(response);
        }

        public async Task<Result<GetTourGuideDetailsResponse>> GetTourGuideByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var guide = await _context.TourGuides
                .Where(g => g.Id == id && !g.IsDeleted)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            if (guide is null)
                return Result.Failure<GetTourGuideDetailsResponse>(TourGuideErrors.TourGuideNotFound);

            var response = guide.Adapt<GetTourGuideDetailsResponse>();
            return Result.Success(response);
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

            // حذف الصورة القديمة لو موجودة
            if (!string.IsNullOrEmpty(guide.ProfilePictureUrl))
            {
                var oldPath = Path.Combine(_webHostEnvironment.WebRootPath, guide.ProfilePictureUrl.TrimStart('/'));
                if (File.Exists(oldPath))
                    File.Delete(oldPath);
            }

            // حفظ الصورة الجديدة
            var fileName = $"{Guid.NewGuid()}{extension}";
            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "tourguides");

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, fileName);
            using var stream = new FileStream(filePath, FileMode.Create);
            await image.CopyToAsync(stream, cancellationToken);

            guide.ProfilePictureUrl = $"/images/tourguides/{fileName}";
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

        public async Task<Result<GetTourGuideDetailsResponse>> UpdateTourGuideAsync(int id,UpdateTourGuideRequest request,CancellationToken cancellationToken = default)
        {
            var guide = await _context.TourGuides
                .FirstOrDefaultAsync(g => g.Id == id && !g.IsDeleted, cancellationToken);

            if (guide is null)
                return Result.Failure<GetTourGuideDetailsResponse>(TourGuideErrors.TourGuideNotFound);

            // تأكد إن الـ email مش موجود عند حد تاني
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

        public IEnumerable<object> GetAllLanguages()
        {
            return Enum.GetValues<Language>()
                .Select(l => new { id = (int)l, name = l.ToString() });
        }
    }
}
