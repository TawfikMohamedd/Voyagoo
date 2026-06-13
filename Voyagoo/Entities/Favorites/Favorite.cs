namespace Voyagoo.Entities.Favorites
{
    public class Favorite
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = default!;

        public int? RestaurantId { get; set; }
        public Voyagoo.Entities.Restaurants.Restaurant? Restaurant { get; set; }

        public int? TourGuideId { get; set; }
        public Voyagoo.Entities.TourGuides.TourGuide? TourGuide { get; set; }

        public int? AttractionId { get; set; }
        public Voyagoo.Entities.Attractions.Attraction? Attraction { get; set; }

        public int? HotelId { get; set; }
        public Voyagoo.Entities.Hotels.Hotel? Hotel { get; set; }

    }

}
