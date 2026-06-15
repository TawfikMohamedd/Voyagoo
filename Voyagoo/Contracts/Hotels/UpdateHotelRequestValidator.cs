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

            RuleFor(x => x.SingleRooms).GreaterThanOrEqualTo(0);
            RuleFor(x => x.DoubleRooms).GreaterThanOrEqualTo(0);
            RuleFor(x => x.TripleRooms).GreaterThanOrEqualTo(0);
            RuleFor(x => x.SuiteRooms).GreaterThanOrEqualTo(0);

            RuleFor(x => x.SinglePrice).GreaterThan(0).WithMessage("Single room price must be greater than 0");
            RuleFor(x => x.DoublePrice).GreaterThan(0).WithMessage("Double room price must be greater than 0");
            RuleFor(x => x.TriplePrice).GreaterThan(0).WithMessage("Triple room price must be greater than 0");
            RuleFor(x => x.SuitePrice).GreaterThan(0).WithMessage("Suite room price must be greater than 0");

            RuleFor(x => x)
                .Must(x => x.SingleRooms + x.DoubleRooms + x.TripleRooms + x.SuiteRooms > 0)
                .WithMessage("Hotel must have at least one room");

            RuleFor(x => x.Discount)
                .InclusiveBetween(0, 100)
                .WithMessage("Discount must be between 0 and 100");

            RuleFor(x => x.ServiceCharge)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Service charge must be 0 or more");
        }
    }
}