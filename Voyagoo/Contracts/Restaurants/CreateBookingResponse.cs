namespace Voyagoo.Contracts.Restaurants
{
    public record CreateBookingResponse(
        int BookingId,
        string RestaurantName,
        string RestaurantAddress,
        DateOnly BookingDate,
        string GuestName,
        string GuestPhone,
        int TablesForTwo,
        int TablesForFour,
        int TablesForSix
    );
}
