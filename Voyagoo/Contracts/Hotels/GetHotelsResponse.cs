namespace Voyagoo.Contracts.Hotels
{
    public record GetHotelsResponse(
        int Id,
        string Name,
        string Description,
        string Location,
        double Rating,
        decimal MinPrice,
        decimal MaxPrice,
        string? MainImageUrl
    );
}
