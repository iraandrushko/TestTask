using System.Globalization;
using CsvHelper.Configuration.Attributes;
using TestTask.Core.Helpers;

namespace TestTask.Core.Models;

public class CabTripModel
{
    [Name(FileColumnMapping.PickupDatetime)]
    [DateTimeStyles(DateTimeStyles.None)]
    public DateTime? PickUpDateTime{ get; init; }

    [Name(FileColumnMapping.DropoffDatetime)]
    [DateTimeStyles(DateTimeStyles.None)]
    public DateTime? DropoffDateTime { get; init; }

    [Name(FileColumnMapping.PassengerCount)]
    [NumberStyles(NumberStyles.Integer)]
    public int? PassengerCount { get; init; }

    [Name(FileColumnMapping.TripDistance)]
    [NumberStyles(NumberStyles.Float)]
    public double? TripDistance { get; init; }

    [Name(FileColumnMapping.StoreAndFwdFlag)]
    public string? StoreAndFwdFlag { get; init; }

    [Name(FileColumnMapping.PULocationID)]
    [NumberStyles(NumberStyles.Integer)]
    public int? PULocationID { get; init; }

    [Name(FileColumnMapping.DOLocationID)]
    [NumberStyles(NumberStyles.Integer)]
    public int? DOLocationID { get; init; }

    [Name(FileColumnMapping.FareAmount)]
    [NumberStyles(NumberStyles.Currency)]
    public decimal? FareAmount { get; init; }

    [Name(FileColumnMapping.TipAmount)]
    [NumberStyles(NumberStyles.Currency)]
    public decimal? TipAmount { get; init; }
}

