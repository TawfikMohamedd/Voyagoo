using Voyagoo.Abstractions;
using Voyagoo.Contracts.Hotels;
using Voyagoo.Entities.Hotels;

namespace Voyagoo.Services
{
    public interface IHotelService
    {
        // Public
        Task<Result<List<GetHotelsResponse>>> GetAllHotelsAsync(CancellationToken cancellationToken = default);
        Task<Result<GetHotelDetailsResponse>> GetHotelByIdAsync(int id, CancellationToken cancellationToken = default);

        // Admin - Hotels
        Task<Result<GetHotelDetailsResponse>> AddHotelAsync(AddHotelRequest request, CancellationToken cancellationToken = default);
        Task<Result> AddHotelImagesAsync(int hotelId, List<IFormFile> images, CancellationToken cancellationToken = default);
        Task<Result> DeleteHotelAsync(int id, CancellationToken cancellationToken = default);
        Task<Result<GetHotelDetailsResponse>> UpdateHotelAsync(int id, UpdateHotelRequest request, CancellationToken cancellationToken = default);
        Task<Result> DeleteHotelImageAsync(int hotelId, int imageId, CancellationToken cancellationToken = default);
        Task<Result> UpdateHotelStatusAsync(int id, HotelStatus status, CancellationToken cancellationToken = default);
        Task<Result<GetHotelsAdminResponse>> GetAllHotelsAdminAsync(CancellationToken cancellationToken = default);
        Task<Result<GetHotelDetailsResponse>> GetHotelByIdAdminAsync(int id, CancellationToken cancellationToken = default);

        // Admin - Features
        Task<Result<List<HotelFeatureResponse>>> GetAllHotelFeaturesAsync(CancellationToken cancellationToken = default);
        Task<Result<HotelFeatureResponse>> AddHotelFeatureAsync(AddHotelFeatureRequest request, CancellationToken cancellationToken = default);

        Task<Result> AddCommentAsync(int hotelId, string userId, AddHotelCommentRequest request, CancellationToken cancellationToken = default);
        Task<Result<GetHotelCommentsResponse>> GetHotelCommentsAsync(int hotelId, CancellationToken cancellationToken = default);
        Task<Result> DeleteCommentAsync(int hotelId, int commentId, CancellationToken cancellationToken = default);
        Task<Result> DeleteOwnCommentAsync(int hotelId, int commentId, string userId, CancellationToken cancellationToken = default);
    }
}
