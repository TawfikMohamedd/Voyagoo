namespace Voyagoo.Contracts.Restaurants
{
    public record AddRestaurantRequest(
        string Name,
        string Description,
        string Address,
        double Rating,
        List<int> FeatureIds
    );
}
