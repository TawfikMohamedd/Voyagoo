namespace Voyagoo.Contracts.Hotels
{
    public record HotelBookingFeatureRequest(
        int BookingFeatureId,
        decimal Price
    );
}
