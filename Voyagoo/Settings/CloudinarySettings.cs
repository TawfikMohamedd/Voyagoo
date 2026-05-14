using System.ComponentModel.DataAnnotations;

namespace Voyagoo.Settings
{
    public class CloudinarySettings
    {
        public static string SectionName = "Cloudinary";

        [Required]
        public string CloudName { get; init; } = string.Empty;

        [Required]
        public string ApiKey { get; init; } = string.Empty;

        [Required]
        public string ApiSecret { get; init; } = string.Empty;
    }
}
