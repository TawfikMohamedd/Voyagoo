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
        public List<HotelBookingFeature> BookingFeatures { get; set; } = [];
        public int SingleRooms { get; set; }
        public decimal SinglePrice { get; set; }
        public int DoubleRooms { get; set; }
        public decimal DoublePrice { get; set; }
        public int TripleRooms { get; set; }
        public decimal TriplePrice { get; set; }
        public int SuiteRooms { get; set; }
        public decimal SuitePrice { get; set; }
        public decimal Discount { get; set; }
        public decimal ServiceCharge { get; set; }
    }
}
