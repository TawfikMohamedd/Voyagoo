namespace Voyagoo.Contracts.Attractions
{
    public record GetAttractionsResponse(
        int Id,
        string Name,
        string Description,
        double Rating,
        string? MainImageUrl
    );
}
