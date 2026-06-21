namespace Voyagoo.Contracts.BudgetPlanning
{
    public record GetMinimumBudgetResponse(
        int NumberOfDays,
        decimal MinimumTotalBudget,
        decimal MinimumHotelBudget,
        decimal MinimumRestaurantBudget,
        decimal MinimumAttractionBudget
    );
}