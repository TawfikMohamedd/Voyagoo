namespace Voyagoo.Entities.Hotels
{
    public class Hotel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public double Rating { get; set; }
        public bool IsDeleted { get; set; } = false;
        public HotelStatus Status { get; set; } = HotelStatus.Active;

        public List<HotelImage> Images { get; set; } = [];
        public List<HotelFeatureMap> Features { get; set; } = [];
        public List<HotelComment> Comments { get; set; } = [];

    }
}
