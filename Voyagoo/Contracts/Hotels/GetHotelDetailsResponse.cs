namespace Voyagoo.Contracts.Hotels
{
    public record GetHotelDetailsResponse(
        int Id,
        string Name,
        string Description,
        string Location,
        double Rating,
        List<HotelImageResponse> Images,
        List<HotelFeatureResponse> Features,
        List<HotelCommentResponse> Comments
    );

    public record HotelImageResponse(
        int Id,
        string ImageUrl,
        bool IsMain
    );

    public record HotelFeatureResponse(
        int Id,
        string Name,
        string Icon
    );
}
