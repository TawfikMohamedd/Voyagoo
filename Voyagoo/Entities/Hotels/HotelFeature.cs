namespace Voyagoo.Entities.Hotels
{
    public class HotelFeature
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;

        public List<HotelFeatureMap> HotelFeatures { get; set; } = [];
    }
}
