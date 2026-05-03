using Mapster;
using Microsoft.EntityFrameworkCore;
using Voyagoo.Abstractions;
using Voyagoo.Contracts.Attractions;
using Voyagoo.Entities.Attractions;
using Voyagoo.Errors;
using Voyagoo.Persistence;

namespace Voyagoo.Services
{
    public class AttractionService(
        VoyagooDbContext context,
        IWebHostEnvironment webHostEnvironment) : IAttractionService
    {
        private readonly VoyagooDbContext _context = context;
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

        public async Task<Result<List<GetAttractionsResponse>>> GetAllAttractionsAsync(CancellationToken cancellationToken = default)
        {
            var attractions = await _context.Attractions
                .Where(a => !a.IsDeleted)
                .Include(a => a.Images)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var response = attractions.Adapt<List<GetAttractionsResponse>>();
            return Result.Success(response);
        }

        public async Task<Result<GetAttractionDetailsResponse>> GetAttractionByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var attraction = await _context.Attractions
                .Where(a => a.Id == id && !a.IsDeleted)
                .Include(a => a.Images)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            if (attraction is null)
                return Result.Failure<GetAttractionDetailsResponse>(AttractionErrors.AttractionNotFound);

            return Result.Success(attraction.Adapt<GetAttractionDetailsResponse>());
        }

        public async Task<Result<GetAttractionDetailsResponse>> AddAttractionAsync(AddAttractionRequest request, CancellationToken cancellationToken = default)
        {
            var attraction = request.Adapt<Attraction>();

            await _context.Attractions.AddAsync(attraction, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success(attraction.Adapt<GetAttractionDetailsResponse>());
        }

        public async Task<Result> AddAttractionImagesAsync(int id, List<IFormFile> images, CancellationToken cancellationToken = default)
        {
            var attraction = await _context.Attractions
                .Include(a => a.Images)
                .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted, cancellationToken);

            if (attraction is null)
                return Result.Failure(AttractionErrors.AttractionNotFound);

            var hasMainImage = attraction.Images.Any(i => i.IsMain);
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };

            foreach (var image in images)
            {
                var extension = Path.GetExtension(image.FileName).ToLower();

                if (!allowedExtensions.Contains(extension))
                    return Result.Failure(AttractionErrors.InvalidImageFile);

                var fileName = $"{Guid.NewGuid()}{extension}";
                var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "attractions");

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var filePath = Path.Combine(folderPath, fileName);
                using var stream = new FileStream(filePath, FileMode.Create);
                await image.CopyToAsync(stream, cancellationToken);

                var isMain = !hasMainImage;
                hasMainImage = true;

                attraction.Images.Add(new AttractionImage
                {
                    ImageUrl = $"/images/attractions/{fileName}",
                    IsMain = isMain
                });
            }

            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        public async Task<Result> DeleteAttractionAsync(int id, CancellationToken cancellationToken = default)
        {
            var attraction = await _context.Attractions
                .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted, cancellationToken);

            if (attraction is null)
                return Result.Failure(AttractionErrors.AttractionNotFound);

            attraction.IsDeleted = true;
            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }

        public async Task<Result<GetAttractionDetailsResponse>> UpdateAttractionAsync(int id, UpdateAttractionRequest request, CancellationToken cancellationToken = default)
        {
            var attraction = await _context.Attractions
                .Include(a => a.Images)
                .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted, cancellationToken);

            if (attraction is null)
                return Result.Failure<GetAttractionDetailsResponse>(AttractionErrors.AttractionNotFound);

            attraction.Name = request.Name;
            attraction.Description = request.Description;
            attraction.Place = request.Place;
            attraction.DateOfInscription = request.DateOfInscription;
            attraction.TicketPrice = request.TicketPrice;
            attraction.Rating = request.Rating;

            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success(attraction.Adapt<GetAttractionDetailsResponse>());
        }

        public async Task<Result> DeleteAttractionImageAsync(int attractionId, int imageId, CancellationToken cancellationToken = default)
        {
            var attraction = await _context.Attractions
                .Include(a => a.Images)
                .FirstOrDefaultAsync(a => a.Id == attractionId && !a.IsDeleted, cancellationToken);

            if (attraction is null)
                return Result.Failure(AttractionErrors.AttractionNotFound);

            var image = attraction.Images.FirstOrDefault(i => i.Id == imageId);
            if (image is null)
                return Result.Failure(AttractionErrors.ImageNotFound);

            var path = Path.Combine(_webHostEnvironment.WebRootPath, image.ImageUrl.TrimStart('/'));
            if (File.Exists(path)) File.Delete(path);

            attraction.Images.Remove(image);

            if (image.IsMain && attraction.Images.Count > 0)
                attraction.Images.First().IsMain = true;

            await _context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
