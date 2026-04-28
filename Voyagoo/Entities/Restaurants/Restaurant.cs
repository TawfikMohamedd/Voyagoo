namespace Voyagoo.Entities.Restaurants
{
    public class Restaurant
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public double Rating { get; set; }
        public bool IsDeleted { get; set; } = false;


        public CuisineType CuisineType { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }

        public List<RestaurantImage> Images { get; set; } = [];
        public List<RestaurantComment> Comments { get; set; } = [];
        public List<RestaurantFeature> Features { get; set; } = [];
    }
}
