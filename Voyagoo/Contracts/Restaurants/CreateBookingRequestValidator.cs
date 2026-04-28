using FluentValidation;

namespace Voyagoo.Contracts.Restaurants
{
    public class CreateBookingRequestValidator : AbstractValidator<CreateBookingRequest>
    {
        public CreateBookingRequestValidator()
        {
            RuleFor(x => x.BookingDate)
                .NotEmpty()
                .GreaterThan(DateOnly.FromDateTime(DateTime.UtcNow))
                .WithMessage("Booking date must be in the future");

            RuleFor(x => x.GuestName)
                .NotEmpty()
                .Length(3, 100);

            RuleFor(x => x.GuestPhone)
                .NotEmpty()
                .Matches(@"^01[0125][0-9]{8}$")
                .WithMessage("Please enter a valid Egyptian phone number");

            RuleFor(x => x.TablesForTwo).GreaterThanOrEqualTo(0);
            RuleFor(x => x.TablesForFour).GreaterThanOrEqualTo(0);
            RuleFor(x => x.TablesForSix).GreaterThanOrEqualTo(0);

            RuleFor(x => x)
                .Must(x => x.TablesForTwo + x.TablesForFour + x.TablesForSix > 0)
                .WithMessage("You must book at least one table");
        }
    }
}
