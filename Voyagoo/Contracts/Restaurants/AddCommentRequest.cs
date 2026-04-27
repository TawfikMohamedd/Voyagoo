namespace Voyagoo.Contracts.Restaurants
{
    public record AddCommentRequest(
        string Content,
        int Rating
    );
}
