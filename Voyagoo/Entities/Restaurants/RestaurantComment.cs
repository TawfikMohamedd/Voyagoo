namespace Voyagoo.Entities.Restaurants
{
    public class RestaurantComment
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; } = default!;

        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = default!;
    }
}
