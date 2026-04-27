namespace Voyagoo.Entities.Restaurants
{
    public class Feature
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;

        public List<RestaurantFeature> RestaurantFeatures { get; set; } = [];
    }
}
