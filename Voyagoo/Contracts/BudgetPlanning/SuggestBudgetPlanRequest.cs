namespace Voyagoo.Contracts.BudgetPlanning
{
    public record SuggestBudgetPlanRequest(
        decimal TotalBudget,
        int NumberOfDays
    );
}
