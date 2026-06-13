namespace Voyagoo.Contracts.Hotels
{
    public record AddHotelCommentRequest(
        string Content,
        int Rating
    );
}