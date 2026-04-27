using System.ComponentModel.DataAnnotations;

namespace Voyagoo.Settings
{
    public class EmailSettings
    {
        public static string SectionName = "EmailSettings";

        [Required]
        public string SmtpHost { get; init; } = string.Empty;

        [Required]
        public int SmtpPort { get; init; }

        [Required]
        public string SenderEmail { get; init; } = string.Empty;

        [Required]
        public string SenderName { get; init; } = string.Empty;

        [Required]
        public string Password { get; init; } = string.Empty;
    }
}
