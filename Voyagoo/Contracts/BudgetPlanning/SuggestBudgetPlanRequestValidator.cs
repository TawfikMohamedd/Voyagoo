using FluentValidation;
using Voyagoo.Abstractions.Consts;

namespace Voyagoo.Contracts.BudgetPlanning
{
    public class SuggestBudgetPlanRequestValidator : AbstractValidator<SuggestBudgetPlanRequest>
    {
        public SuggestBudgetPlanRequestValidator()
        {
            RuleFor(x => x.TotalBudget)
                .GreaterThan(0)
                .WithMessage("Total budget must be greater than 0");

            RuleFor(x => x.NumberOfDays)
                .GreaterThan(0)
                .WithMessage("Number of days must be at least 1");
        }
    }
}
