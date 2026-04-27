namespace Voyagoo.Contracts.Account
{
    public record UpdateProfileRequest(
        string FirstName,
        string LastName,
        string? PhoneNumber
    );
}
