using FluentValidation;

namespace Voyagoo.Contracts.Restaurants
{
    public class UpdateRestaurantRequestValidator : AbstractValidator<UpdateRestaurantRequest>
    {
        public UpdateRestaurantRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .Length(3, 200);

            RuleFor(x => x.Description)
                .NotEmpty()
                .Length(10, 2000);

            RuleFor(x => x.Address)
                .NotEmpty()
                .Length(5, 500);

            RuleFor(x => x.Rating)
                .InclusiveBetween(1.0, 5.0)
                .WithMessage("Rating must be between 1 and 5");

            RuleFor(x => x.CuisineType)
                .IsInEnum()
                .WithMessage("Invalid cuisine type");

            RuleFor(x => x.MinPrice)
                .GreaterThan(0)
                .WithMessage("Min price must be greater than 0");

            RuleFor(x => x.MaxPrice)
                .GreaterThan(x => x.MinPrice)
                .WithMessage("Max price must be greater than Min price");

            RuleFor(x => x.TablesForTwo)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Tables for two must be 0 or more");

            RuleFor(x => x.TablesForFour)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Tables for four must be 0 or more");

            RuleFor(x => x.TablesForSix)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Tables for six must be 0 or more");

            RuleFor(x => x)
                .Must(x => x.TablesForTwo + x.TablesForFour + x.TablesForSix > 0)
                .WithMessage("Restaurant must have at least one table");

            RuleFor(x => x.FeatureIds)
                .NotNull();
        }
    }
}
