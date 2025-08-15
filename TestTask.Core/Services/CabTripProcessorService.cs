using System.Globalization;
using CsvHelper;
using TestTask.Core.Models;
using TestTask.Core.Services.Interface;
using TestTask.Core.Validators;

namespace TestTask.Core.Services;

public sealed class CsvCabTripProcessor
{
    private readonly Config _config;
    private readonly CabTripValidator _validator;
    private readonly IDatabaseService<CabTripModel> _dataInserter;
    private readonly INormalizerService<CabTripModel> _normalizerService;
    private readonly IFileServiceProvider<CabTripModel> _fileServiceProvider;
    private readonly HashSet<(DateTime pickupUtc, DateTime dropoffUtc, int? pax)> _seen = new();

    public CsvCabTripProcessor(
        Config config,
        CabTripValidator validator,
        IDatabaseService<CabTripModel> dataInserter,
        INormalizerService<CabTripModel> normalizerService,
        IFileServiceProvider<CabTripModel> fileServiceProvider)
    {
        _config = config;
        _validator = validator;
        _dataInserter = dataInserter;
        _normalizerService = normalizerService;
        _fileServiceProvider = fileServiceProvider;
    }

    public async Task<ProcessingResult> Process()
    {
        var result = new ProcessingResult();
        var items = new List<CabTripModel>();

        using var reader = new StreamReader(_config.SourceCsvPath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        using var duplicatesWriter = await _fileServiceProvider.GetFileServiceAsync(_config.DuplicatesCsvPath);
        using var errorsWriter = await _fileServiceProvider.GetFileServiceAsync(_config.ErrorCsvPath);

        if (!csv.Read() || !csv.ReadHeader())
            return result;


        while (csv.Read())
        {
            var model = csv.GetRecord<CabTripModel>();

            var validationResult = _validator.Validate(model);
            if (!validationResult.IsValid)
            {
                await errorsWriter.WriteItemAsync(model);
                result.ErrorsCount++;
                continue;
            }

            var normalized = _normalizerService.Normalize(model);

            var key = (normalized.PickUpDateTime!.Value,
                       normalized.DropoffDateTime!.Value,
                       normalized.PassengerCount);

            if (!_seen.Add(key))
            {
                await duplicatesWriter.WriteItemAsync(model);
                result.DuplicatesCount++;
                continue;
            }

            items.Add(normalized);

            if (items.Count >= _config.BatchSize)
            {
                await _dataInserter.BulkInsert(items);
                result.ProcessedCount += _config.BatchSize;

                items.Clear();
            }
        }

        await _dataInserter.BulkInsert(items);
        result.ProcessedCount += items.Count;

        return result;
    }
}
