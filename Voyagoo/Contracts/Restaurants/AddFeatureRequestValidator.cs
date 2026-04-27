using FluentValidation;

namespace Voyagoo.Contracts.Restaurants
{
    public class AddFeatureRequestValidator : AbstractValidator<AddFeatureRequest>
    {
        public AddFeatureRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .Length(2, 100);

            RuleFor(x => x.Icon)
                .NotEmpty()
                .MaximumLength(50);
        }
    }
}
