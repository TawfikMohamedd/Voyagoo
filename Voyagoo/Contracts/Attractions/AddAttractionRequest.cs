namespace Voyagoo.Contracts.Attractions
{
    public record AddAttractionRequest(
        string Name,
        string Description,
        string Place,
        DateOnly DateOfInscription,
        decimal TicketPrice,
        double Rating
    );
}
