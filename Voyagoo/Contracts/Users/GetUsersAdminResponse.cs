namespace Voyagoo.Contracts.Users
{
    public record GetUsersAdminResponse(
        int TotalUsers,
        int ActiveUsers,
        int InactiveUsers,
        List<UserAdminItem> Users
    );

    public record UserAdminItem(
        string Id,
        string FullName,
        string Email,
        string? PhoneNumber,
        bool IsActive
    );
}