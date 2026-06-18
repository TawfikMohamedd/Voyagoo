namespace Voyagoo.Contracts.Account
{
    public record GetAllBookingsResponse(
        List<HotelBookingHistoryItem> HotelBookings,
        List<TourGuideBookingHistoryItem> TourGuideBookings,
        List<RestaurantBookingHistoryItem> RestaurantBookings
    );

    public record HotelBookingHistoryItem(
        int BookingId,
        string HotelName,
        DateOnly CheckIn,
        DateOnly CheckOut,
        int Nights,
        decimal TotalPrice,
        string PaymentType,
        string Status,
        DateTime CreatedAt,
        string? MainImageUrl
    );

    public record TourGuideBookingHistoryItem(
        int BookingId,
        string TourGuideName,
        DateOnly BookingDate,
        int NumberOfDays,
        decimal TotalPrice,
        string PaymentType,
        string Status,
        DateTime CreatedAt,
        string? ProfilePictureUrl
    );

    public record RestaurantBookingHistoryItem(
    int BookingId,
    string RestaurantName,
    string RestaurantAddress,
    DateOnly BookingDate,
    string GuestName,
    string GuestPhone,
    int TablesForTwo,
    int TablesForFour,
    int TablesForSix,
    DateTime CreatedAt,
    string? MainImageUrl
);
}