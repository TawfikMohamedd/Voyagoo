namespace Voyagoo.Contracts.TourGuides
{
    public record CreateTourGuideBookingRequest(
        DateOnly BookingDate,
         int NumberOfDays
    );
}
