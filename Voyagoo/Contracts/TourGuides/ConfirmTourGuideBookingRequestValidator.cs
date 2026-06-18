using FluentValidation;

namespace Voyagoo.Contracts.TourGuides
{
    public class ConfirmTourGuideBookingRequestValidator : AbstractValidator<ConfirmTourGuideBookingRequest>
    {
        public ConfirmTourGuideBookingRequestValidator()
        {
            RuleFor(x => x.PaymentType)
                .NotEmpty()
                .Must(x => x == "cash on arrival" || x == "card")
                .WithMessage("PaymentType must be either 'cash on arrival' or 'card'");
        }
    }
}
