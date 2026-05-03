using Voyagoo.Abstractions;

namespace Voyagoo.Errors
{
    public static class TourGuideBookingErrors
    {
        public static readonly Error TourGuideNotAvailable =
            new("TourGuideBooking.NotAvailable",
                "Tour guide is already booked during this period",
                StatusCodes.Status400BadRequest);
    }
}
