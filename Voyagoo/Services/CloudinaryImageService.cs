using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using Voyagoo.Settings;

namespace Voyagoo.Services
{
    public class CloudinaryImageService(IOptions<CloudinarySettings> options) : IImageService
    {
        private readonly Cloudinary _cloudinary = new(new Account(
            options.Value.CloudName,
            options.Value.ApiKey,
            options.Value.ApiSecret
        ));

        public async Task<string> UploadImageAsync(IFormFile image, string folder, CancellationToken cancellationToken = default)
        {
            await using var stream = image.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(image.FileName, stream),
                Folder = folder,
                UseFilename = false,
                UniqueFilename = true,
                Overwrite = false
            };

            var result = await _cloudinary.UploadAsync(uploadParams, cancellationToken);

            if (result.Error is not null)
                throw new Exception(result.Error.Message);

            return result.SecureUrl.ToString();
        }

        public async Task DeleteImageAsync(string imageUrl)
        {
            // استخرج الـ PublicId من الـ URL
            // مثال URL: https://res.cloudinary.com/cloud/image/upload/v123/voyagoo/restaurants/abc123.jpg
            // الـ PublicId هيبقى: voyagoo/restaurants/abc123

            var uri = new Uri(imageUrl);
            var segments = uri.AbsolutePath.Split('/');

            // ابدأ من بعد "upload" في الـ path
            var uploadIndex = Array.IndexOf(segments, "upload");
            if (uploadIndex < 0) return;

            // اشيل الـ version segment (بيبدأ بـ v + رقم) لو موجود
            var relevantSegments = segments.Skip(uploadIndex + 1)
                .SkipWhile(s => s.StartsWith('v') && s.Length > 1 && s[1..].All(char.IsDigit))
                .ToArray();

            // اشيل الـ extension
            var lastSegment = Path.GetFileNameWithoutExtension(relevantSegments.Last());
            relevantSegments[^1] = lastSegment;

            var publicId = string.Join("/", relevantSegments);

            var deleteParams = new DeletionParams(publicId);
            await _cloudinary.DestroyAsync(deleteParams);
        }
    }
}
