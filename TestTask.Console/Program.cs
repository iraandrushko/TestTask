using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestTask.Core;
using TestTask.Core.Models;
using TestTask.Core.Services;
using TestTask.Core.Services.Interface;
using TestTask.Core.Validators;

namespace TestTask.Console;

public class Program
{
    static async Task Main(string[] args)
    {
        var builder = new ConfigurationBuilder();

        builder.SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        IConfiguration config = builder.Build();

        var processingConfigs = config.GetSection("Config").Get<Config>();

        var serviceProvider = new ServiceCollection()
            .AddSingleton(provider => processingConfigs!)
            .AddScoped<IDatabaseService<CabTripModel>, CabTripDatabaseService>()
            .AddScoped<INormalizerService<CabTripModel>, CabTripNormalizerService>()
            .AddScoped<IFileServiceProvider<CabTripModel>, CabTripFileServiceProvider>()
            .AddScoped<CabTripValidator>()
            .AddScoped<CsvCabTripProcessor>()
            .BuildServiceProvider();

        var svc = serviceProvider.GetRequiredService<CsvCabTripProcessor>();


        System.Console.WriteLine("Processing ...");

        var processResults = await svc.Process();

        System.Console.WriteLine($"Process count: {processResults.ProcessedCount};");
        System.Console.WriteLine($"Duplicates count: {processResults.DuplicatesCount};");
        System.Console.WriteLine($"Errors count: {processResults.ErrorsCount};");

        
    }
}
