namespace Voyagoo.Contracts.TourGuides
{
    public record UpdateTourGuideRequest(
        string Name,
        string Email,
        string PhoneNumber,
        string Description,
        double Rating,
        decimal PricePerDay,
        List<int> Languages
    );
}
