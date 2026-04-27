namespace Voyagoo.Entities.Restaurants
{
    public class RestaurantFeature
    {
        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; } = default!;

        public int FeatureId { get; set; }
        public Feature Feature { get; set; } = default!;
    }
}
