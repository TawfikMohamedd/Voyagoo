namespace Voyagoo.Contracts.BudgetPlanning
{
    public record BudgetPlanResponse(
        int Id,
        decimal TotalBudget,
        int NumberOfDays,
        decimal HotelBudget,
        decimal RestaurantBudget,
        decimal AttractionBudget,
        BudgetPlanHotelItem? Hotel,
        List<BudgetPlanRestaurantItem> Restaurants,
        List<BudgetPlanAttractionItem> Attractions,
        decimal TotalHotelCost,
        decimal TotalRestaurantCost,
        decimal TotalAttractionCost,
        decimal TotalEstimatedCost,
        DateTime CreatedAt
    );

    public record BudgetPlanHotelItem(
        int? Id,
        string Name,
        decimal Price,
        string? MainImageUrl
    );

    public record BudgetPlanRestaurantItem(
        int? Id,
        string Name,
        decimal EstimatedPrice,
        string? MainImageUrl
    );

    public record BudgetPlanAttractionItem(
        int? Id,
        string Name,
        decimal TicketPrice,
        string? MainImageUrl
    );
}
