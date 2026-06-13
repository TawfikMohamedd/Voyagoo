namespace Voyagoo.Contracts.Hotels
{
    public record AddHotelRequest(
        string Name,
        string Description,
        string Location,
        double Rating,
        List<int> FeatureIds
    );
}
