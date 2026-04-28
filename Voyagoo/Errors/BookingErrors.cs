using Voyagoo.Abstractions;

namespace Voyagoo.Errors
{
    public static class BookingErrors
    {
        public static readonly Error NotEnoughTablesForTwo =
            new("Booking.NotEnoughTablesForTwo", "Not enough tables for two available on this date", StatusCodes.Status400BadRequest);

        public static readonly Error NotEnoughTablesForFour =
            new("Booking.NotEnoughTablesForFour", "Not enough tables for four available on this date", StatusCodes.Status400BadRequest);

        public static readonly Error NotEnoughTablesForSix =
            new("Booking.NotEnoughTablesForSix", "Not enough tables for six available on this date", StatusCodes.Status400BadRequest);
    }
}
