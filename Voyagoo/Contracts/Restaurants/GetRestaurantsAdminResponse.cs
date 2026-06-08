namespace Voyagoo.Contracts.Restaurants
{
    public record GetRestaurantsAdminResponse(
        int TotalRestaurants,
        int ActiveRestaurants,
        int InactiveRestaurants,
        List<RestaurantAdminItem> Restaurants
    );

    public record RestaurantAdminItem(
        int Id,
        string Name,
        string CuisineType,
        double Rating,
         string PriceRange,
        string Status,
        int TotalTables,
        string? MainImageUrl
    );
}
