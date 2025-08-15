using System.Globalization;
using CsvHelper;
using FluentValidation;
using TestTask.Core.Helpers;
using TestTask.Core.Models;
using TestTask.Core.Services;
using TestTask.Core.Services.Interface;

namespace TestTask.Core.Processing;

public sealed class CsvCabTripProcessor_Copy
{
    private readonly CabTripValidator _validator;
    private readonly IDatabaseService<CabTripModel> _dataInserter;
    private readonly Config _config;
    private readonly HashSet<(DateTime pickupUtc, DateTime dropoffUtc, int? pax)> _seen = new();
    
    public List<CabTripModel> Errors { get; } = new();
    public List<CabTripModel> Duplicates { get; } = new();

    public CsvCabTripProcessor_Copy(
        Config config,
        CabTripValidator validator,
        IDatabaseService<CabTripModel> dataInserter)
    {
        _config = config;
        _validator = validator;
        _dataInserter = dataInserter;
    }

    public async Task Process()
    {
        var items = new List<CabTripModel>();

        using var reader = new StreamReader(_config.SourceCsvPath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        if (!csv.Read() || !csv.ReadHeader())
            return;


        while (csv.Read())
        {
            var sPickup = csv.GetField(FileColumnMapping.PickupDatetime);
            var sDropoff = csv.GetField(FileColumnMapping.DropoffDatetime);
            var sPax = csv.GetField(FileColumnMapping.PassengerCount);
            var sDist = csv.GetField(FileColumnMapping.TripDistance);
            var sFlag = csv.GetField(FileColumnMapping.StoreAndFwdFlag);
            var sPU = csv.GetField(FileColumnMapping.PULocationID);
            var sDO = csv.GetField(FileColumnMapping.DOLocationID);
            var sFare = csv.GetField(FileColumnMapping.FareAmount);
            var sTip = csv.GetField(FileColumnMapping.TipAmount);

            var model = new CabTripModel
            {
                PickUpDateTime = TryParseDate(sPickup),
                DropoffDateTime = TryParseDate(sDropoff),
                PassengerCount = TryParseInt(sPax),
                TripDistance = TryParseDouble(sDist),
                StoreAndFwdFlag = sFlag,
                PULocationID = TryParseInt(sPU),
                DOLocationID = TryParseInt(sDO),
                FareAmount = TryParseDecimal(sFare),
                TipAmount = TryParseDecimal(sTip)
            };

            if (_validator is not null)
            {
                var res = _validator.Validate(model);
                if (!res.IsValid)
                {
                    Errors.Add(model);
                    continue;
                }
            }

            var normalized = CabTripNormalizerService.Normalize(model);

            var key = (normalized.PickUpDateTime!.Value,
                       normalized.DropoffDateTime!.Value,
                       normalized.PassengerCount);

            if (!_seen.Add(key))
            {
                Duplicates.Add(normalized);
                continue;
            }

            items.Add(normalized);

            if(items.Count >= _config.BatchSize)
            {
                await _dataInserter.BulkInsert(items);

                items.Clear();
            }
        }

        await _dataInserter.BulkInsert(items);
        items.Clear();

        return;
    }

    private static DateTime? TryParseDate(string? s)
        => DateTime.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.None, out var v) ? v : (DateTime?)null;

    private static int? TryParseInt(string? s)
        => int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out var v) ? v : null;

    private static double? TryParseDouble(string? s)
        => double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out var v) ? v : null;

    private static decimal? TryParseDecimal(string? s)
        => decimal.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out var v) ? v : null;
}
