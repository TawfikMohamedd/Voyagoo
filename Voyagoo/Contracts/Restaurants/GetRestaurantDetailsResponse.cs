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
        List<string> ImageUrls,
        List<FeatureResponse> Features,
        List<CommentResponse> Comments
    );

    public record FeatureResponse(
        int Id,
        string Name,
        string Icon
    );

    public record CommentResponse(
        int Id,
        string UserName,
        string Content,
        int Rating,
        DateTime CreatedAt
    );
}
