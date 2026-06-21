namespace Voyagoo.Contracts.BudgetPlanning
{
    public record SuggestBudgetPlanResponse(
        decimal TotalBudget,
        int NumberOfDays,
        decimal HotelBudget,
        decimal RestaurantBudget,
        decimal AttractionBudget,
        List<SuggestedHotelItem> SuggestedHotels,
        List<SuggestedRestaurantItem> SuggestedRestaurants,
        List<SuggestedAttractionItem> SuggestedAttractions
    );

    public record SuggestedHotelItem(
        int Id,
        string Name,
        string Location,
        double Rating,
        decimal MinPrice,
        decimal EstimatedTotalPrice,
        string? MainImageUrl
    );

    public record SuggestedRestaurantItem(
        int Id,
        string Name,
        string Address,
        double Rating,
        string CuisineType,
        decimal MinPrice,
        decimal MaxPrice,
        decimal EstimatedPrice,
        string? MainImageUrl
    );

    public record SuggestedAttractionItem(
        int Id,
        string Name,
        string Location,
        double Rating,
        string Category,
        decimal TicketPrice,
        string? MainImageUrl
    );
}
