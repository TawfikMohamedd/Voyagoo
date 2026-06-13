namespace Voyagoo.Entities.Hotels
{
    public class HotelImage
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsMain { get; set; } = false;

        public int HotelId { get; set; }
        public Hotel Hotel { get; set; } = default!;
    }
}
