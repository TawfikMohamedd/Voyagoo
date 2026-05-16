namespace Voyagoo.Contracts.Restaurants
{
    public record GetRestaurantCommentsResponse(
        int TotalComments,
        double AverageRating,
        List<CommentResponse> Comments
    );
}
