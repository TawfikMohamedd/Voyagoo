using FluentValidation;

namespace Voyagoo.Contracts.TourGuides
{
    public class CreateTourGuideBookingRequestValidator : AbstractValidator<CreateTourGuideBookingRequest>
    {
        public CreateTourGuideBookingRequestValidator()
        {
            RuleFor(x => x.BookingDate)
            .NotEmpty()
            .GreaterThan(DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("Booking date must be in the future");

            RuleFor(x => x.NumberOfDays)
                .GreaterThan(0)
                .WithMessage("Number of days must be at least 1");
        }
    }
}
