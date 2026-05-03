namespace Voyagoo.Entities.Attractions
{
    public class AttractionImage
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsMain { get; set; } = false;

        public int AttractionId { get; set; }
        public Attraction Attraction { get; set; } = default!;
    }
}
