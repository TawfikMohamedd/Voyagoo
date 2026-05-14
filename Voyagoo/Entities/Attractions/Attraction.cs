namespace Voyagoo.Entities.Attractions
{
    public class Attraction
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public int YearOfInscription { get; set; }
        public decimal TicketPrice { get; set; }
        public double Rating { get; set; }
        public bool IsDeleted { get; set; } = false;
        public AttractionStatus Status { get; set; } = AttractionStatus.Active;
        public AttractionCategory Category { get; set; } = AttractionCategory.Historical;

        public List<AttractionImage> Images { get; set; } = [];
    }
}
