using FluentValidation;

namespace Voyagoo.Contracts.Hotels
{
    public class AddHotelFeatureRequestValidator : AbstractValidator<AddHotelFeatureRequest>
    {
        public AddHotelFeatureRequestValidator()
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
