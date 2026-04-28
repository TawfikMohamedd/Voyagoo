namespace Voyagoo.Entities.Restaurants
{
    public class Booking
    {
        public int Id { get; set; }
        public DateOnly BookingDate { get; set; }
        public string GuestName { get; set; } = string.Empty;
        public string GuestPhone { get; set; } = string.Empty;

        public int TablesForTwo { get; set; }
        public int TablesForFour { get; set; }
        public int TablesForSix { get; set; }

        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; } = default!;

        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = default!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
