namespace Voyagoo.Contracts.Account
{
    public record GetProfileResponse(
        string FirstName,
        string LastName,
        string Email,
        string? PhoneNumber,
        string? ProfilePictureUrl
    );
}
