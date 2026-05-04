namespace Voyagoo.Contracts.Attractions
{
    public record GetAttractionsAdminResponse(
        int TotalAttractions,
        int ActiveAttractions,
        int InactiveAttractions,
        List<AttractionAdminItem> Attractions
    );

    public record AttractionAdminItem(
        int Id,
        string Name,
        string Place,
        double Rating,
        decimal TicketPrice,
        string Status
    );
}
