namespace Voyagoo.Entities.Hotels
{
    public class HotelBookingRoom
    {
        public int Id { get; set; }

        public int HotelBookingId { get; set; }
        public HotelBooking HotelBooking { get; set; } = default!;

        public RoomType RoomType { get; set; }
        public int Quantity { get; set; }
        public decimal PricePerNight { get; set; }
    }
}