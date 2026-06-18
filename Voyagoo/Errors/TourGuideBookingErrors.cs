using Voyagoo.Abstractions;

namespace Voyagoo.Errors
{
    public static class TourGuideBookingErrors
    {
        public static readonly Error TourGuideNotAvailable =
            new("TourGuideBooking.NotAvailable",
                "Tour guide is already booked during this period",
                StatusCodes.Status400BadRequest);


        public static readonly Error BookingNotFound =
    new("TourGuideBooking.NotFound", "Booking not found", StatusCodes.Status404NotFound);

        public static readonly Error BookingNotOwned =
            new("TourGuideBooking.NotOwned", "You can only confirm your own bookings", StatusCodes.Status403Forbidden);

        public static readonly Error BookingAlreadyConfirmed =
            new("TourGuideBooking.AlreadyConfirmed", "This booking has already been confirmed", StatusCodes.Status400BadRequest);


    }
}
