namespace Voyagoo.Contracts.Hotels
{
    public record GetBookingHistoryResponse(
        List<BookingHistoryItem> Pending,
        List<BookingHistoryItem> Completed
    );

    public record BookingHistoryItem(
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
        decimal TotalPrice,
        string PaymentType,
        string Status,
        DateTime CreatedAt
    );
}