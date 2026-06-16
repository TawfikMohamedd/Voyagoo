namespace Voyagoo.Entities.Hotels
{
    public class HotelBooking
    {
        public int Id { get; set; }

        public int HotelId { get; set; }
        public Hotel Hotel { get; set; } = default!;

        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = default!;

        public DateOnly CheckIn { get; set; }
        public DateOnly CheckOut { get; set; }
        public int Nights { get; set; }

        public decimal RoomsTotal { get; set; }
        public decimal BoardsTotal { get; set; }
        public decimal ExtrasTotal { get; set; }
        public decimal Subtotal { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal ServiceChargePercentage { get; set; }
        public decimal ServiceChargeAmount { get; set; }
        public decimal TotalPrice { get; set; }

        public List<HotelBookingRoom> Rooms { get; set; } = [];
        public List<HotelBookingFeatureSelection> SelectedFeatures { get; set; } = [];

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
