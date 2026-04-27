namespace Voyagoo.Contracts.Authentication
{
    public record RefreshTokenRequest
    (
        string Token,
        string RefreshToken

        );
}
