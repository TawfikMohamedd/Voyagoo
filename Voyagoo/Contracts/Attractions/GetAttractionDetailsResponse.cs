namespace Voyagoo.Contracts.Attractions
{
    public record GetAttractionDetailsResponse(
        int Id,
        string Name,
        string Description,
        string Location,
        int YearOfInscription,
        decimal TicketPrice,
        double Rating,
        string Category,
        List<AttractionImageResponse> Images
    );

    public record AttractionImageResponse(
        int Id,
        string ImageUrl,
        bool IsMain
    );


}
