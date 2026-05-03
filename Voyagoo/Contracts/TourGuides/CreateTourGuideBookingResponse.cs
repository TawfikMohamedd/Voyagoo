namespace Voyagoo.Contracts.TourGuides
{
    public record CreateTourGuideBookingResponse(
        int BookingId,
        string TourGuideName,
        DateOnly BookingDate,
        int NumberOfDays,
        decimal PricePerDay,
        decimal TotalPrice
    );
}
