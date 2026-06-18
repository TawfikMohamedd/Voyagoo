using FluentValidation;

namespace Voyagoo.Contracts.Hotels
{
    public class ConfirmHotelBookingRequestValidator : AbstractValidator<ConfirmHotelBookingRequest>
    {
        public ConfirmHotelBookingRequestValidator()
        {
            RuleFor(x => x.PaymentType)
                .NotEmpty()
                .Must(x => x == "cash on arrival" || x == "card")
                .WithMessage("PaymentType must be either 'cash on arrival' or 'card'");
        }
    }
}