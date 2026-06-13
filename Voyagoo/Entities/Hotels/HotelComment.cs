namespace Voyagoo.Entities.Hotels
{
    public class HotelComment
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int HotelId { get; set; }
        public Hotel Hotel { get; set; } = default!;

        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = default!;
    }
}
