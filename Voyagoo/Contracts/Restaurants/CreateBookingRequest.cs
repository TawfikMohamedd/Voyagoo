namespace Voyagoo.Contracts.Restaurants
{
    public record CreateBookingRequest(
        DateOnly BookingDate,
        string GuestName,
        string GuestPhone,
        int TablesForTwo,
        int TablesForFour,
        int TablesForSix
    );
}
