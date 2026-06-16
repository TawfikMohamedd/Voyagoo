namespace Voyagoo.Entities.Hotels
{
    public class HotelBookingFeatureSelection
    {
        public int Id { get; set; }

        public int HotelBookingId { get; set; }
        public HotelBooking HotelBooking { get; set; } = default!;

        public int BookingFeatureId { get; set; }
        public BookingFeature BookingFeature { get; set; } = default!;

        public int RoomsCount { get; set; }
        public decimal PricePerNight { get; set; }
    }
}
