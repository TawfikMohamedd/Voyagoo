namespace Voyagoo.Contracts.Restaurants
{
    public record GetRestaurantDetailsResponse(
        int Id,
        string Name,
        string Description,
        string Address,
        double Rating,
        string CuisineType,        
        decimal MinPrice,           
        decimal MaxPrice,
        int TablesForTwo,
        int TablesForFour,
        int TablesForSix,
        List<RestaurantImageResponse> Images,
        List<FeatureResponse> Features,
        List<CommentResponse> Comments
    );

    public record RestaurantImageResponse(
    int Id,
    string ImageUrl,
    bool IsMain
    );

    public record FeatureResponse(
        int Id,
        string Name,
        string Icon
    );

    public record CommentResponse(
        int Id,
        string UserName,
        string? ProfilePictureUrl,
        string Content,
        int Rating,
        DateOnly CreatedAt
    );
}
