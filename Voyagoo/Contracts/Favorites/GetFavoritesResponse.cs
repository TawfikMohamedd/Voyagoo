namespace Voyagoo.Contracts.Favorites
{
    public record GetFavoritesResponse(
        List<FavoriteRestaurantItem> Restaurants,
        List<FavoriteTourGuideItem> TourGuides,
        List<FavoriteAttractionItem> Attractions,
        List<FavoriteHotelItem> Hotels
    );

    public record FavoriteRestaurantItem(
        int Id,
        string Name,
        string CuisineType,
        double Rating,
        string? MainImageUrl
    );

    public record FavoriteTourGuideItem(
        int Id,
        string Name,
        double Rating,
        string? ProfilePictureUrl
    );

    public record FavoriteAttractionItem(
        int Id,
        string Name,
        string Location,
        double Rating,
        string? MainImageUrl
    );

    public record FavoriteHotelItem(
    int Id,
    string Name,
    string Location,
    double Rating,
    string? MainImageUrl
);
}
