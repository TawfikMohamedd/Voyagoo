namespace Voyagoo.Contracts.Hotels
{
    public record GetHotelCommentsResponse(
        int TotalComments,
        double AverageRating,
        List<HotelCommentResponse> Comments
    );

    public record HotelCommentResponse(
        int Id,
        string UserName,
        string? ProfilePictureUrl,
        string Content,
        int Rating,
        DateOnly CreatedAt
    );
}