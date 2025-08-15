using TestTask.Core.Helpers;
using TestTask.Core.Models;

namespace TestTask.Core.Services;

public class CabTripFileService : IFileService<CabTripModel>, IDisposable
{
    private string path;
    private StreamWriter? streamWriter;
    private FileStream? fileStream;

    public CabTripFileService(string path)
    {
        this.path = path;
    }

    public async Task InitAsync() 
    {
        fileStream = new FileStream(
            path, 
            FileMode.Create, 
            FileAccess.Write, 
            FileShare.Read, 
            bufferSize: 64 * 1024, 
            useAsync: true);

        streamWriter = new StreamWriter(fileStream);

        await streamWriter.WriteLineAsync(string.Join(',', FileColumnMapping.ColumnTypeMap.Keys));

        await streamWriter.FlushAsync();
    }

    public async Task WriteItemAsync(CabTripModel item, CancellationToken ct = default)
    {
        string line = GetLine(item);

        await streamWriter!.WriteLineAsync(line);
        await streamWriter.FlushAsync();
    }

    private static string GetLine(CabTripModel m)
    {
        return string.Join(',', new[] {
            m.PickUpDateTime?.ToString() ?? string.Empty,
            m.DropoffDateTime?.ToString() ?? string.Empty,
            m.PassengerCount?.ToString() ?? string.Empty,
            m.TripDistance?.ToString() ?? string.Empty,
            m.StoreAndFwdFlag ?? string.Empty,
            m.PULocationID?.ToString() ?? string.Empty,
            m.DOLocationID?.ToString() ?? string.Empty,
            m.FareAmount?.ToString() ?? string.Empty,
            m.TipAmount?.ToString() ?? string.Empty,
        });
    }

    public void Dispose()
    {
        streamWriter?.Dispose();
        fileStream?.Dispose();
    }
}
