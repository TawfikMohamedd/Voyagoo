namespace Voyagoo.Contracts.Hotels
{
    public record CreateHotelBookingResponse(
        int BookingId,
        string HotelName,
        DateOnly CheckIn,
        DateOnly CheckOut,
        int Nights,
        List<BookedRoomItem> Rooms,
        List<BookedFeatureItem> Features,
        decimal RoomsTotal,
        decimal BoardsTotal,
        decimal ExtrasTotal,
        decimal Subtotal,
        decimal DiscountPercentage,
        decimal DiscountAmount,
        decimal ServiceChargePercentage,
        decimal ServiceChargeAmount,
        decimal TotalPrice
    );

    public record BookedRoomItem(
        string RoomType,
        int Quantity,
        decimal PricePerNight,
        decimal Total
    );

    public record BookedFeatureItem(
        int BookingFeatureId,
        string Name,
        string Icon,
        int RoomsCount,
        decimal PricePerNight,
        decimal Total
    );
}