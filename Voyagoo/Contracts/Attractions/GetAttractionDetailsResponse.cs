namespace Voyagoo.Contracts.Attractions
{
    public record GetAttractionDetailsResponse(
        int Id,
        string Name,
        string Description,
        string Place,
        DateOnly DateOfInscription,
        decimal TicketPrice,
        double Rating,
        List<AttractionImageResponse> Images
    );

    public record AttractionImageResponse(
        int Id,
        string ImageUrl,
        bool IsMain
    );


}
