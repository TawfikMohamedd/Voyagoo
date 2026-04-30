namespace Voyagoo.Contracts.TourGuides
{
    public record AddTourGuideRequest(
        string Name,
        string Email,
        string PhoneNumber,
        string Description,
        double Rating,
        List<int> Languages  
    );
}
