namespace Voyagoo.Contracts.Favorites
{
    public record GetFavoritesResponse(
        List<FavoriteRestaurantItem> Restaurants,
        List<FavoriteTourGuideItem> TourGuides,
        List<FavoriteAttractionItem> Attractions
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
        string Place,
        double Rating,
        string? MainImageUrl
    );
}
