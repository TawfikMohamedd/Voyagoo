namespace Voyagoo.Contracts.TourGuides
{
    public record GetTourGuidesAdminResponse(
        int TotalTourGuides,
        int ActiveTourGuides,
        int InactiveTourGuides,
        List<TourGuideAdminItem> TourGuides
    );

    public record TourGuideAdminItem(
        int Id,
        string Name,
        string Email,
        string PhoneNumber,
        string Languages,
        double Rating,
        string Status,
        string? ProfilePictureUrl
    );
}
