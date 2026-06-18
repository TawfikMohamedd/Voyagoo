using Voyagoo.Abstractions;

namespace Voyagoo.Errors
{
    public static class HotelBookingErrors
    {
        public static readonly Error HotelNotFound =
            new("HotelBooking.HotelNotFound", "Hotel not found", StatusCodes.Status404NotFound);

        public static readonly Error InvalidRoomType =
            new("HotelBooking.InvalidRoomType", "One or more selected room types are not available in this hotel", StatusCodes.Status400BadRequest);

        public static readonly Error InvalidFeature =
            new("HotelBooking.InvalidFeature", "One or more selected features are not available in this hotel", StatusCodes.Status400BadRequest);




        public static readonly Error BookingNotFound =
            new("HotelBooking.NotFound", "Booking not found", StatusCodes.Status404NotFound);

        public static readonly Error BookingNotOwned =
            new("HotelBooking.NotOwned", "You can only confirm your own bookings", StatusCodes.Status403Forbidden);

        public static readonly Error BookingAlreadyConfirmed =
            new("HotelBooking.AlreadyConfirmed", "This booking has already been confirmed", StatusCodes.Status400BadRequest);
    }
}