namespace Voyagoo.Contracts.Home
{
    public record GetHomeResponse(
        List<HomeOfferItem> Offers,
        RecommendedSection Recommended,
        List<AvailableThisWeekItem> AvailableThisWeek
    );

    // ── Section 1: Offers ──
    public record HomeOfferItem(
        int Id,
        string Name,
        string Location,
        double Rating,
        decimal MinPrice,
        decimal MaxPrice,
        decimal Discount,
        string? MainImageUrl
    );

    // ── Section 2: Recommended For You ──
    public record RecommendedSection(
        List<RecommendedHotelItem> Hotels,
        List<RecommendedRestaurantItem> Restaurants,
        List<RecommendedAttractionItem> Attractions
    );

    public record RecommendedHotelItem(
        int Id,
        string Name,
        string Location,
        double Rating,
        decimal MinPrice,
        decimal MaxPrice,
        string? MainImageUrl
    );

    public record RecommendedRestaurantItem(
        int Id,
        string Name,
        string Address,
        double Rating,
        string CuisineType,
        decimal MinPrice,
        decimal MaxPrice,
        string? MainImageUrl
    );

    public record RecommendedAttractionItem(
        int Id,
        string Name,
        string Location,
        double Rating,
        string Category,
        string? MainImageUrl
    );

    // ── Section 3: Available This Week ──
    public record AvailableThisWeekItem(
        int Id,
        string Name,
        string Type,
        string Location,
        double Rating,
        decimal? MinPrice,
        decimal? MaxPrice,
        string? MainImageUrl
    );
}