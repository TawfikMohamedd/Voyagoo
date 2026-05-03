using FluentValidation;

namespace Voyagoo.Contracts.Attractions
{
    public class UpdateAttractionRequestValidator : AbstractValidator<UpdateAttractionRequest>
    {
        public UpdateAttractionRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .Length(3, 200);

            RuleFor(x => x.Description)
                .NotEmpty()
                .Length(10, 2000);

            RuleFor(x => x.Place)
                .NotEmpty()
                .Length(3, 300);

            RuleFor(x => x.DateOfInscription)
                .NotEmpty()
                .LessThan(DateOnly.FromDateTime(DateTime.UtcNow))
                .WithMessage("Date of inscription must be in the past");

            RuleFor(x => x.TicketPrice)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Ticket price must be 0 or more");

            RuleFor(x => x.Rating)
                .InclusiveBetween(1.0, 5.0)
                .WithMessage("Rating must be between 1 and 5");
        }
    }
}
