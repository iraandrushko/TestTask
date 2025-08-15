namespace TestTask.Core.Services.Interface;

public interface IFileServiceProvider<T> where T : class
{
    Task<IFileService<T>> GetFileServiceAsync(string path);
}
