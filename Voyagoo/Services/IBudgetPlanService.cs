using Voyagoo.Abstractions;
using Voyagoo.Contracts.BudgetPlanning;

namespace Voyagoo.Services
{
    public interface IBudgetPlanService
    {
        Task<Result<GetMinimumBudgetResponse>> GetMinimumBudgetAsync(
            GetMinimumBudgetRequest request,
            CancellationToken cancellationToken = default);

        Task<Result<SuggestBudgetPlanResponse>> SuggestPlanAsync(
            SuggestBudgetPlanRequest request,
            CancellationToken cancellationToken = default);

        Task<Result<BudgetPlanResponse>> SavePlanAsync(
            string userId,
            SaveBudgetPlanRequest request,
            CancellationToken cancellationToken = default);

        Task<Result<List<BudgetPlanResponse>>> GetUserPlansAsync(
            string userId,
            CancellationToken cancellationToken = default);

        Task<Result> DeletePlanAsync(
            string userId,
            int planId,
            CancellationToken cancellationToken = default);
    }
}