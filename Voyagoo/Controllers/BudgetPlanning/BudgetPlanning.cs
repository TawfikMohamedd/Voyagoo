using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Voyagoo.Abstractions;
using Voyagoo.Contracts.BudgetPlanning;
using Voyagoo.Services;

namespace Voyagoo.Controllers.BudgetPlanning
{
    [Route("budget-planning")]
    [ApiController]
    [Authorize]
    public class BudgetPlanningController(IBudgetPlanService budgetPlanService) : ControllerBase
    {
        private readonly IBudgetPlanService _budgetPlanService = budgetPlanService;

        [HttpGet("minimum-budget")]
        public async Task<IActionResult> GetMinimumBudget([FromQuery] int numberOfDays, CancellationToken cancellationToken)
        {
            var request = new GetMinimumBudgetRequest(numberOfDays);
            var result = await _budgetPlanService.GetMinimumBudgetAsync(request, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpPost("suggest")]
        public async Task<IActionResult> Suggest([FromBody] SuggestBudgetPlanRequest request, CancellationToken cancellationToken)
        {
            var result = await _budgetPlanService.SuggestPlanAsync(request, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] SaveBudgetPlanRequest request, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null) return Unauthorized();

            var result = await _budgetPlanService.SavePlanAsync(userId, request, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpGet("")]
        public async Task<IActionResult> GetMyPlans(CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null) return Unauthorized();

            var result = await _budgetPlanService.GetUserPlansAsync(userId, cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlan(int id, CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null) return Unauthorized();

            var result = await _budgetPlanService.DeletePlanAsync(userId, id, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }
    }
}