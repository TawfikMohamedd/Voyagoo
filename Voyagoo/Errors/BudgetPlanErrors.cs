using Voyagoo.Abstractions;

namespace Voyagoo.Errors
{
    public static class BudgetPlanErrors
    {
        public static readonly Error InvalidNumberOfDays =
            new("BudgetPlan.InvalidNumberOfDays", "Number of days must be at least 1", StatusCodes.Status400BadRequest);

        public static readonly Error BudgetBelowMinimum =
            new("BudgetPlan.BudgetBelowMinimum", "Total budget is below the minimum required for the selected number of days", StatusCodes.Status400BadRequest);

        public static readonly Error InvalidHotelSelection =
            new("BudgetPlan.InvalidHotelSelection", "Selected hotel is not available or does not exist", StatusCodes.Status400BadRequest);

        public static readonly Error HotelExceedsBudget =
            new("BudgetPlan.HotelExceedsBudget", "Selected hotel's total price exceeds the allocated hotel budget", StatusCodes.Status400BadRequest);

        public static readonly Error InvalidRestaurantSelection =
            new("BudgetPlan.InvalidRestaurantSelection", "One or more selected restaurants are not available or do not exist", StatusCodes.Status400BadRequest);

        public static readonly Error NoRestaurantsSelected =
            new("BudgetPlan.NoRestaurantsSelected", "You must select at least one restaurant", StatusCodes.Status400BadRequest);

        public static readonly Error RestaurantsExceedBudget =
            new("BudgetPlan.RestaurantsExceedBudget", "Selected restaurants' total estimated price exceeds the allocated restaurant budget", StatusCodes.Status400BadRequest);

        public static readonly Error InvalidAttractionSelection =
            new("BudgetPlan.InvalidAttractionSelection", "One or more selected attractions are not available or do not exist", StatusCodes.Status400BadRequest);

        public static readonly Error NoAttractionsSelected =
            new("BudgetPlan.NoAttractionsSelected", "You must select at least one attraction", StatusCodes.Status400BadRequest);

        public static readonly Error AttractionsExceedBudget =
            new("BudgetPlan.AttractionsExceedBudget", "Selected attractions' total ticket price exceeds the allocated attraction budget", StatusCodes.Status400BadRequest);

        public static readonly Error BudgetPlanNotFound =
            new("BudgetPlan.NotFound", "Budget plan not found", StatusCodes.Status404NotFound);

        public static readonly Error BudgetPlanNotOwned =
            new("BudgetPlan.NotOwned", "You can only access your own budget plans", StatusCodes.Status403Forbidden);
    }
}