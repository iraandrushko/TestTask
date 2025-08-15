using TestTask.Core.Models;
using TestTask.Core.Services.Interface;

namespace TestTask.Core.Services;

public class CabTripNormalizerService : INormalizerService<CabTripModel>
{
    public CabTripModel Normalize(CabTripModel source)
    {
        string? flag = source.StoreAndFwdFlag?.Trim() switch
        {
            "Y" or "y" => "Yes",
            "N" or "n" => "No",
            _ => null
        };

        DateTime? pickupUtc = source.PickUpDateTime.HasValue
           ? ToUtcFromEst(source.PickUpDateTime.Value)
           : (DateTime?)null;

        DateTime? dropoffUtc = source.DropoffDateTime.HasValue
            ? ToUtcFromEst(source.DropoffDateTime.Value)
            : (DateTime?)null;


        return new CabTripModel
        {
            PickUpDateTime = pickupUtc,
            DropoffDateTime = dropoffUtc,
            PassengerCount = source.PassengerCount,
            TripDistance = source.TripDistance,
            StoreAndFwdFlag = flag,
            PULocationID = source.PULocationID,
            DOLocationID = source.DOLocationID,
            FareAmount = source.FareAmount,
            TipAmount = source.TipAmount
        };
    }

    private static DateTime ToUtcFromEst(DateTime estLocal)
    {
        var tz = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
        var unspecified = DateTime.SpecifyKind(estLocal, DateTimeKind.Unspecified);
        return TimeZoneInfo.ConvertTimeToUtc(unspecified, tz);
    }
}
