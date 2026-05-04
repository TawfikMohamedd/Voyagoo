using Voyagoo.Entities.Restaurants;

namespace Voyagoo.Contracts.Restaurants
{
    public record UpdateRestaurantRequest(
        string Name,
        string Description,
        string Address,
        double Rating,
        CuisineType CuisineType,
        decimal MinPrice,
        decimal MaxPrice,
        int TablesForTwo,
        int TablesForFour,
        int TablesForSix,
        RestaurantStatus Status,
        List<int> FeatureIds
    );
}
