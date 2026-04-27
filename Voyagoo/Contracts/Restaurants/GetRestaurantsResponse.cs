namespace Voyagoo.Contracts.Restaurants
{
    public record GetRestaurantsResponse(
        int Id,
        string Name,
        string Description,
        double Rating,
        string? MainImageUrl
    );
}
