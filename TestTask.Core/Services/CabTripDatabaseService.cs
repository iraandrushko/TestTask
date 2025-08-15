using System.Data;
using Microsoft.Data.SqlClient;
using TestTask.Core.Helpers;
using TestTask.Core.Models;
using TestTask.Core.Services.Interface;

namespace TestTask.Core.Services;

public class CabTripDatabaseService : IDatabaseService<CabTripModel>
{
    private readonly string _connectionString;

    public CabTripDatabaseService(Config config)
    {
        _connectionString = config.ConnectionString;
    }

    public async Task BulkInsert(IEnumerable<CabTripModel> items)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (var bulkCopy = new SqlBulkCopy(connection))
            {
                bulkCopy.DestinationTableName = "CabTrips";

                foreach (var key in FileColumnMapping.ColumnTypeMap.Keys) 
                {
                    bulkCopy.ColumnMappings.Add(key, key);
                }

                using (var dataTable = new DataTable())
                {
                    foreach (var column in FileColumnMapping.ColumnTypeMap)
                    {
                        dataTable.Columns.Add(column.Key, column.Value);
                    }

                    foreach (var trip in items)
                    {
                        var row = dataTable.NewRow();
                        row[FileColumnMapping.PickupDatetime] = trip.PickUpDateTime;
                        row[FileColumnMapping.DropoffDatetime] = trip.DropoffDateTime;
                        row[FileColumnMapping.PassengerCount] = trip.PassengerCount;
                        row[FileColumnMapping.TripDistance] = trip.TripDistance;
                        row[FileColumnMapping.StoreAndFwdFlag] = trip.StoreAndFwdFlag;
                        row[FileColumnMapping.PULocationID] = trip.PULocationID;
                        row[FileColumnMapping.DOLocationID] = trip.DOLocationID;
                        row[FileColumnMapping.FareAmount] = trip.FareAmount;
                        row[FileColumnMapping.TipAmount] = trip.TipAmount;

                        dataTable.Rows.Add(row);
                    }

                    await bulkCopy.WriteToServerAsync(dataTable);
                }
            }
        }
    }


}
