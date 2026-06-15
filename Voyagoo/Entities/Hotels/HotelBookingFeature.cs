namespace Voyagoo.Entities.Hotels
{
    public class HotelBookingFeature
    {
        public int HotelId { get; set; }
        public Hotel Hotel { get; set; } = default!;

        public int BookingFeatureId { get; set; }
        public BookingFeature BookingFeature { get; set; } = default!;

        public decimal Price { get; set; }
    }
}
