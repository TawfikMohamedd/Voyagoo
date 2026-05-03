using FluentValidation;
using Voyagoo.Entities.TourGuides;

namespace Voyagoo.Contracts.TourGuides
{
    public class UpdateTourGuideRequestValidator : AbstractValidator<UpdateTourGuideRequest>
    {
        public UpdateTourGuideRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .Length(3, 200);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .Matches(@"^01[0125][0-9]{8}$")
                .WithMessage("Please enter a valid Egyptian phone number");

            RuleFor(x => x.Description)
                .NotEmpty()
                .Length(10, 2000);

            RuleFor(x => x.Rating)
                .InclusiveBetween(1.0, 5.0)
                .WithMessage("Rating must be between 1 and 5");

            RuleFor(x => x.PricePerDay)
                .GreaterThan(0)
                .WithMessage("Price per day must be greater than 0");

            RuleFor(x => x.Languages)
                .NotEmpty()
                .WithMessage("At least one language is required")
                .Must(langs => langs.All(l => Enum.IsDefined(typeof(Language), l)))
                .WithMessage("One or more selected languages are invalid");
        }
    }
}
