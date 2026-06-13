namespace Voyagoo.Entities.Hotels
{
    public class HotelFeatureMap
    {
        public int HotelId { get; set; }
        public Hotel Hotel { get; set; } = default!;

        public int HotelFeatureId { get; set; }
        public HotelFeature HotelFeature { get; set; } = default!;
    }
}