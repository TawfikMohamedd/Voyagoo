namespace Voyagoo.Entities.TourGuides
{
    public class TourGuideBooking
    {
        public int Id { get; set; }
        public DateOnly BookingDate { get; set; }
        public int NumberOfDays { get; set; }
        public decimal TotalPrice { get; set; }
        public int TourGuideId { get; set; }
        public TourGuide TourGuide { get; set; } = default!;

        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = default!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
