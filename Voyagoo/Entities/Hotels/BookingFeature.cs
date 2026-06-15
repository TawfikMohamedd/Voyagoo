namespace Voyagoo.Entities.Hotels
{
    public class BookingFeature
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;

        public List<HotelBookingFeature> HotelBookingFeatures { get; set; } = [];
    }
}