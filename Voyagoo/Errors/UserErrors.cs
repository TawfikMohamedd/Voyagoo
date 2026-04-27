using Voyagoo.Abstractions;

namespace Voyagoo.Errors
{
    public static class UserErrors
    {
        public static readonly Error InvalidCredentials =
            new("User.InvalidCredentials", "Invalid email/password", StatusCodes.Status401Unauthorized);

        public static readonly Error InvalidJwtToken =
            new("User.InvalidJwtToken", "Invalid Jwt token", StatusCodes.Status401Unauthorized);

        public static readonly Error InvalidRefreshToken =
            new("User.InvalidRefreshToken", "Invalid refresh token", StatusCodes.Status401Unauthorized);

        public static readonly Error DuplicatedEmail =
            new("User.DuplicatedEmail", "Another user with the same email is already exists", StatusCodes.Status409Conflict);

        public static readonly Error EmailNotConfirmed =
            new("User.EmailNotConfirmed", "Email is not confirmed", StatusCodes.Status401Unauthorized);

        public static readonly Error InvalidCode =
            new("User.InvalidCode", "Invalid code", StatusCodes.Status401Unauthorized);

        public static readonly Error DuplicatedConfirmation =
            new("User.DuplicatedConfirmation", "Email already confirmed", StatusCodes.Status400BadRequest);

        public static readonly Error EmailNotFound =
              new("User.EmailNotFound", "No account found with this email", StatusCodes.Status404NotFound);

        public static readonly Error InvalidOrExpiredOtp =
            new("User.InvalidOrExpiredOtp", "OTP code is invalid or has expired", StatusCodes.Status400BadRequest);

        public static readonly Error OtpNotVerified =
            new("User.OtpNotVerified", "Please verify your OTP before resetting the password", StatusCodes.Status400BadRequest);

        public static readonly Error InvalidImageFile =
    new("User.InvalidImageFile", "Invalid image file. Allowed: jpg, jpeg, png, webp", StatusCodes.Status400BadRequest);
    }
}
