namespace Voyagoo.Contracts.TourGuides
{
    public record GetTourGuideBookingHistoryResponse(
        List<TourGuideBookingHistoryItem> Pending,
        List<TourGuideBookingHistoryItem> Completed
    );

    public record TourGuideBookingHistoryItem(
        int BookingId,
        string TourGuideName,
        DateOnly BookingDate,
        int NumberOfDays,
        decimal PricePerDay,
        decimal TotalPrice,
        string PaymentType,
        string Status,
        DateTime CreatedAt
    );
}