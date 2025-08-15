namespace TestTask.Core.Services.Interface;

public interface IDatabaseService<T>
{
    Task BulkInsert(IEnumerable<T> items);
}
