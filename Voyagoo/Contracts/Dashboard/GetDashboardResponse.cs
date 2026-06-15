namespace Voyagoo.Contracts.Dashboard
{
    public record GetDashboardResponse(
        DashboardOverview Overview,
        List<TopRestaurantItem> TopRestaurants,
        List<TopTourGuideItem> TopTourGuides,
        List<TopHotelItem> TopHotels,
        List<TopAttractionItem> TopAttractions,
        List<RecentUserItem> RecentUsers
    );

    public record DashboardOverview(
        int TotalRestaurants,
        int TotalTourGuides,
        int TotalAttractions,
        int TotalUsers,
        int TotalHotels
    );

    public record TopRestaurantItem(
        int Id,
        string Name,
        string CuisineType,
        double Rating,
        string Status
    );

    public record TopTourGuideItem(
        int Id,
        string Name,
        double Rating,
        string Status
    );
    public record TopHotelItem(
        int Id,
        string Name,
        string Location,
        double Rating,
        string Status
    );
    public record TopAttractionItem(
    int Id,
    string Name,
    string Category,
    double Rating,
    string Status
);

    public record RecentUserItem(
        string Id,
        string FullName,
        string Email,
        DateTime CreatedAt
    );
}
