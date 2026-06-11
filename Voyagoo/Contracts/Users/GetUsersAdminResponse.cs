namespace Voyagoo.Contracts.Users
{
    public record GetUsersAdminResponse(
        int TotalUsers,
        List<UserAdminItem> Users
    );

    public record UserAdminItem(
        string Id,
        string FirstName,
        string LastName,
        string Email,
        string? PhoneNumber
    );
}