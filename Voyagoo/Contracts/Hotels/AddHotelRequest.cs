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
        List<int> FeatureIds
    );
}
