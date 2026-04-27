using Voyagoo.Abstractions;
using Voyagoo.Contracts.Authentication;
using Voyagoo.Contracts.Authentication.Register;

namespace Voyagoo.Services
{
    public interface IAuthService
    {
        Task<AuthResponse?> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default);
        Task<AuthResponse?> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default);
        Task<bool> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default);

        Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default);





        Task<Result> SendResetPasswordOtpAsync(string email, CancellationToken cancellationToken = default);
        Task<Result> VerifyOtpAsync(string email, string code, CancellationToken cancellationToken = default);
        Task<Result> ResetPasswordAsync(string email, string newPassword, CancellationToken cancellationToken = default);

    }
}
