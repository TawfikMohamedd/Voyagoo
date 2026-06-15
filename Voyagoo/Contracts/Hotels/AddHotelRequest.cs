namespace Voyagoo.Contracts.Hotels
{
    public record AddHotelRequest(
        string Name,
        string Description,
        string Location,
        double Rating,
        int SingleRooms,
        decimal SinglePrice,
        int DoubleRooms,
        decimal DoublePrice,
        int TripleRooms,
        decimal TriplePrice,
        int SuiteRooms,
        decimal SuitePrice,
        decimal Discount,
        decimal ServiceCharge,
        decimal FullBoardPrice,
        decimal HalfBoardPrice,
        List<int> FeatureIds,
        List<HotelBookingFeatureRequest> BookingFeatures
    );
}
