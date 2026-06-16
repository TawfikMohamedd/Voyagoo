using FluentValidation;
using Voyagoo.Abstractions.Consts;

namespace Voyagoo.Contracts.Hotels
{
    public class CreateHotelBookingRequestValidator : AbstractValidator<CreateHotelBookingRequest>
    {
        public CreateHotelBookingRequestValidator()
        {
            RuleFor(x => x.CheckIn)
                .NotEmpty()
                .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
                .WithMessage("Check-in date must be today or in the future");

            RuleFor(x => x.CheckOut)
                .NotEmpty()
                .GreaterThan(x => x.CheckIn)
                .WithMessage("Check-out date must be after check-in date");

            RuleFor(x => x.Rooms)
                .NotNull()
                .Must(rooms => rooms.Count > 0)
                .WithMessage("You must select at least one room");

            RuleForEach(x => x.Rooms).ChildRules(room =>
            {
                room.RuleFor(r => r.RoomType)
                    .InclusiveBetween(1, 4)
                    .WithMessage("Invalid room type. Valid values: 1=Single, 2=Double, 3=Triple, 4=Suite");

                room.RuleFor(r => r.Quantity)
                    .GreaterThan(0)
                    .WithMessage("Room quantity must be greater than 0");
            });

            RuleFor(x => x.Rooms)
                .Must(rooms => rooms.Select(r => r.RoomType).Distinct().Count() == rooms.Count)
                .WithMessage("Duplicate room types are not allowed");

            RuleFor(x => x.FullBoardRooms)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.HalfBoardRooms)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x)
                .Must(x => (x.FullBoardRooms + x.HalfBoardRooms) == x.Rooms.Sum(r => r.Quantity))
                .WithMessage("FullBoardRooms + HalfBoardRooms must equal the total number of booked rooms");

            RuleFor(x => x.ExtraFeatures)
                .NotNull();

            RuleForEach(x => x.ExtraFeatures).ChildRules(feature =>
            {
                feature.RuleFor(f => f.BookingFeatureId)
                    .GreaterThan(0);

                feature.RuleFor(f => f.RoomsCount)
                    .GreaterThan(0)
                    .WithMessage("RoomsCount for a feature must be greater than 0");
            });

            RuleFor(x => x)
                .Must(x => x.ExtraFeatures.All(f => f.RoomsCount <= x.Rooms.Sum(r => r.Quantity)))
                .WithMessage("RoomsCount for any feature cannot exceed total booked rooms");

            RuleFor(x => x.ExtraFeatures)
                .Must(features => features.Select(f => f.BookingFeatureId).Distinct().Count() == features.Count)
                .WithMessage("Duplicate features are not allowed");

            RuleFor(x => x.ExtraFeatures)
                .Must(features => features.All(f =>
                    f.BookingFeatureId != DefaultBookingFeatures.FullBoardId &&
                    f.BookingFeatureId != DefaultBookingFeatures.HalfBoardId))
                .WithMessage("Full Board and Half Board must not be included in ExtraFeatures");
        }
    }
}