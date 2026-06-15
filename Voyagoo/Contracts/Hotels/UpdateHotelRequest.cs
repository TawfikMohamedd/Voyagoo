namespace Voyagoo.Contracts.Hotels
{
    public record UpdateHotelRequest(
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
        List<int> FeatureIds,
        List<HotelBookingFeatureRequest> BookingFeatures
    );
}
