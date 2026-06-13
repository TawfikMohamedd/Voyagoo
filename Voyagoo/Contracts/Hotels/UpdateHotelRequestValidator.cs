using FluentValidation;

namespace Voyagoo.Contracts.Hotels
{
    public class UpdateHotelRequestValidator : AbstractValidator<UpdateHotelRequest>
    {
        public UpdateHotelRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .Length(3, 200);

            RuleFor(x => x.Description)
                .NotEmpty()
                .Length(10, 2000);

            RuleFor(x => x.Location)
                .NotEmpty()
                .Length(3, 300);

            RuleFor(x => x.Rating)
                .InclusiveBetween(1.0, 5.0)
                .WithMessage("Rating must be between 1 and 5");

            RuleFor(x => x.FeatureIds)
                .NotNull();
        }
    }
}