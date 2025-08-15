namespace TestTask.Core.Models;

public class ProcessingResult
{
    public int ProcessedCount { get; set; }
    public int DuplicatesCount { get; set; }
    public int ErrorsCount { get; set; }
}