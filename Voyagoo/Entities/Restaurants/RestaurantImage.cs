namespace Voyagoo.Entities.Restaurants
{
    public class RestaurantImage
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsMain { get; set; } = false;

        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; } = default!;
    }
}
