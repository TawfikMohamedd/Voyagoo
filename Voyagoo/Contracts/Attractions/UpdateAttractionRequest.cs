namespace Voyagoo.Contracts.Attractions
{
    public record UpdateAttractionRequest(
        string Name,
        string Description,
        string Place,
        DateOnly DateOfInscription,
        decimal TicketPrice,
        double Rating
    );
}
