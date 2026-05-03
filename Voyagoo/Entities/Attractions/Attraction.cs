namespace Voyagoo.Entities.Attractions
{
    public class Attraction
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Place { get; set; } = string.Empty;
        public DateOnly DateOfInscription { get; set; }
        public decimal TicketPrice { get; set; }
        public double Rating { get; set; }
        public bool IsDeleted { get; set; } = false;

        public List<AttractionImage> Images { get; set; } = [];
    }
}
