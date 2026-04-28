using FluentValidation;

namespace Voyagoo.Contracts.Restaurants
{
    public class AddRestaurantRequestValidator : AbstractValidator<AddRestaurantRequest>
    {
        public AddRestaurantRequestValidator()
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

            RuleFor(x => x.FeatureIds)
                .NotNull();
        }
    }
}
