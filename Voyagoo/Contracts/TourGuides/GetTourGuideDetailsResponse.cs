namespace Voyagoo.Contracts.TourGuides
{
    public record GetTourGuideDetailsResponse(
        int Id,
        string Name,
        string Email,
        string PhoneNumber,
        string Description,
        double Rating,
        string? ProfilePictureUrl,
        List<string> Languages
    );
}
