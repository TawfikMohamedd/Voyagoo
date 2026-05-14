using Voyagoo.Entities.Attractions;

namespace Voyagoo.Contracts.Attractions
{
    public record AddAttractionRequest(
        string Name,
        string Description,
        string Location,
        int YearOfInscription,
        decimal TicketPrice,
        double Rating,
        AttractionCategory Category
    );
}
