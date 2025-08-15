namespace TestTask.Core.Services;

public interface IFileService<T> : IDisposable where T : class
{
    Task WriteItemAsync(T item, CancellationToken ct = default);

    Task InitAsync();
}
