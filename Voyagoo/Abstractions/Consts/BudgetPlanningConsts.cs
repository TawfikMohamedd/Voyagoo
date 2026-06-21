namespace Voyagoo.Abstractions.Consts
{
    public static class BudgetPlanningConsts
    {
        public const decimal HotelPercentage = 0.60m;
        public const decimal RestaurantPercentage = 0.30m;
        public const decimal AttractionPercentage = 0.10m;

        // Fallback only — used when the database has no hotels/restaurants/attractions
        // to calculate a real minimum from (e.g. a fresh/empty database).
        public const decimal FallbackMinimumBudgetPerDay = 500m;
    }
}