namespace Voyagoo.Contracts.BudgetPlanning
{
    public record SaveBudgetPlanRequest(
        decimal TotalBudget,
        int NumberOfDays,
        int HotelId,
        List<int> RestaurantIds,
        List<int> AttractionIds
    );
}