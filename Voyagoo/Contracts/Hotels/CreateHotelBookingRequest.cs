namespace Voyagoo.Contracts.Hotels
{
    public record CreateHotelBookingRequest(
        DateOnly CheckIn,
        DateOnly CheckOut,
        List<RoomSelectionRequest> Rooms,
        int FullBoardRooms,
        int HalfBoardRooms,
        List<ExtraFeatureSelectionRequest> ExtraFeatures
    );

    public record RoomSelectionRequest(
        int RoomType,
        int Quantity
    );

    public record ExtraFeatureSelectionRequest(
        int BookingFeatureId,
        int RoomsCount
    );
}