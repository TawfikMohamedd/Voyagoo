using Voyagoo.Entities.Restaurants;

namespace Voyagoo.Contracts.Restaurants
{
    public record AddRestaurantRequest(
        string Name,
        string Description,
        string Address,
        double Rating,
        CuisineType CuisineType,   
        decimal MinPrice,            
        decimal MaxPrice,
        List<int> FeatureIds
    );
}
