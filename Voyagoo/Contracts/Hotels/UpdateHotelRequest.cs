namespace Voyagoo.Contracts.Hotels
{
    public record UpdateHotelRequest(
        string Name,
        string Description,
        string Location,
        double Rating,
        List<int> FeatureIds
    );
}
