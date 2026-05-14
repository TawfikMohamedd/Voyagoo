namespace Voyagoo.Services
{
    public interface IImageService
    {
        Task<string> UploadImageAsync(IFormFile image, string folder, CancellationToken cancellationToken = default);
        Task DeleteImageAsync(string imageUrl);
    }
}
