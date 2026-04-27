using Voyagoo.Entities;

namespace Voyagoo.Authentication
{
    public interface IJwtProvider
    {
        (string token, int expiresIn) GenerateToken(ApplicationUser user, IList<string> roles);
        string? ValidateToken(string token);
    }
}
