namespace Voyagoo.Contracts.Dashboard
{
    public record GetDashboardResponse(
        DashboardOverview Overview,
        List<TopRestaurantItem> TopRestaurants,
        List<TopTourGuideItem> TopTourGuides
    );

    public record DashboardOverview(
        int TotalRestaurants,
        int TotalTourGuides,
        int TotalAttractions,
        int TotalUsers
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
}
