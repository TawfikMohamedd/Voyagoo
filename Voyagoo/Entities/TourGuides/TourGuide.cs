namespace Voyagoo.Entities.TourGuides
{
    public class TourGuide
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Rating { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public decimal PricePerDay { get; set; }
        public bool IsDeleted { get; set; } = false;
        public TourGuideStatus Status { get; set; } = TourGuideStatus.Active;

        public List<Language> Languages { get; set; } = [];
        public List<TourGuideBooking> Bookings { get; set; } = [];
    }
}
