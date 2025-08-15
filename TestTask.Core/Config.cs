namespace TestTask.Core;

public class Config
{
    public int BatchSize { get; set; }
    public required string ConnectionString { get; set; }
    public required string SourceCsvPath { get; set; }
    public required string ErrorCsvPath { get; set; }
    public required string DuplicatesCsvPath { get; set; }
}
