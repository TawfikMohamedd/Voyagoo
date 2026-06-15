using Microsoft.AspNetCore.Identity;

namespace Voyagoo.Entities
{
    public sealed class ApplicationUser :IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? PasswordResetOtp { get; set; }
        public DateTime? PasswordResetOtpExpiry { get; set; }
        public bool IsOtpVerified { get; set; } = false;
        public string? ProfilePictureUrl { get; set; }
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<RefreshToken> RefreshTokens { get; set; } = [];







    }


    
}
