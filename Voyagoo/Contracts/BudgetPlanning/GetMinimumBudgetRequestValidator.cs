using FluentValidation;

namespace Voyagoo.Contracts.BudgetPlanning
{
    public class GetMinimumBudgetRequestValidator : AbstractValidator<GetMinimumBudgetRequest>
    {
        public GetMinimumBudgetRequestValidator()
        {
            RuleFor(x => x.NumberOfDays)
                .GreaterThan(0)
                .WithMessage("Number of days must be at least 1");
        }
    }
}