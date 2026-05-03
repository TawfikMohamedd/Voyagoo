using Voyagoo.Abstractions;
using Voyagoo.Contracts.Attractions;

namespace Voyagoo.Services
{
    public interface IAttractionService
    {
        Task<Result<List<GetAttractionsResponse>>> GetAllAttractionsAsync(CancellationToken cancellationToken = default);
        Task<Result<GetAttractionDetailsResponse>> GetAttractionByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<Result<GetAttractionDetailsResponse>> AddAttractionAsync(AddAttractionRequest request, CancellationToken cancellationToken = default);
        Task<Result> AddAttractionImagesAsync(int id, List<IFormFile> images, CancellationToken cancellationToken = default);
        Task<Result> DeleteAttractionAsync(int id, CancellationToken cancellationToken = default);
        Task<Result<GetAttractionDetailsResponse>> UpdateAttractionAsync(int id, UpdateAttractionRequest request, CancellationToken cancellationToken = default);

        Task<Result> DeleteAttractionImageAsync(int attractionId, int imageId, CancellationToken cancellationToken = default);
    }
}
