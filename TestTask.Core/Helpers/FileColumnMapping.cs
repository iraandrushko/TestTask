namespace TestTask.Core.Helpers;

public static class FileColumnMapping
{
    public const string PickupDatetime = "tpep_pickup_datetime";
    public const string DropoffDatetime = "tpep_dropoff_datetime";
    public const string PassengerCount = "passenger_count";
    public const string TripDistance = "trip_distance";
    public const string StoreAndFwdFlag = "store_and_fwd_flag";
    public const string PULocationID = "PULocationID";
    public const string DOLocationID = "DOLocationID";
    public const string FareAmount = "fare_amount";
    public const string TipAmount = "tip_amount";

    public static IDictionary<string, Type> ColumnTypeMap = new Dictionary<string, Type>
    {
        { PickupDatetime, typeof(DateTime) },
        { DropoffDatetime, typeof(DateTime) },
        { PassengerCount, typeof(int) },
        { TripDistance, typeof(double) },
        { StoreAndFwdFlag, typeof(string) },
        { PULocationID, typeof(int) },
        { DOLocationID, typeof(int) },
        { FareAmount, typeof(decimal) },
        { TipAmount, typeof(decimal) },
    };
}
