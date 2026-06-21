using FluentValidation;
using Voyagoo.Abstractions.Consts;
using Voyagoo.Contracts.BudgetPlanning;

public class SaveBudgetPlanRequestValidator : AbstractValidator<SaveBudgetPlanRequest>
{
    public SaveBudgetPlanRequestValidator()
    {
        RuleFor(x => x.TotalBudget)
            .GreaterThan(0)
            .WithMessage("Total budget must be greater than 0");

        RuleFor(x => x.NumberOfDays)
            .GreaterThan(0)
            .WithMessage("Number of days must be at least 1");

        RuleFor(x => x.HotelId)
            .GreaterThan(0)
            .WithMessage("You must select a hotel");

        RuleFor(x => x.RestaurantIds)
            .NotNull()
            .Must(ids => ids.Count > 0)
            .WithMessage("You must select at least one restaurant");

        RuleFor(x => x.RestaurantIds)
            .Must(ids => ids.Distinct().Count() == ids.Count)
            .WithMessage("Duplicate restaurant ids are not allowed");

        RuleFor(x => x.AttractionIds)
            .NotNull()
            .Must(ids => ids.Count > 0)
            .WithMessage("You must select at least one attraction");

        RuleFor(x => x.AttractionIds)
            .Must(ids => ids.Distinct().Count() == ids.Count)
            .WithMessage("Duplicate attraction ids are not allowed");
    }
}
