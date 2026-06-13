namespace Voyagoo.Contracts.Hotels
{
    public record GetHotelsAdminResponse(
        int TotalHotels,
        int ActiveHotels,
        int InactiveHotels,
        List<HotelAdminItem> Hotels
    );

    public record HotelAdminItem(
        int Id,
        string Name,
        string Location,
        double Rating,
        string Status,
        string? MainImageUrl
    );
}
