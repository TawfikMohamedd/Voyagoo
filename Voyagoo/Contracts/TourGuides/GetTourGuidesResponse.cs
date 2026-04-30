namespace Voyagoo.Contracts.TourGuides
{
    public record GetTourGuidesResponse(
        int Id,
        string Name,
        string Description,
        double Rating,
        string? ProfilePictureUrl
    );
}
