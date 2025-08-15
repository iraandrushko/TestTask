# ETL: CSV → MS SQL (CabTrips)

A simple ETL project that reads cab trip data from CSV, validates & normalizes rows, deduplicates in-memory, and bulk-inserts into MS SQL Server. Outputs duplicates and errors to CSV.

## Tech Stack

* .NET console app with DI
* `CsvHelper`, `FluentValidation`
* `Microsoft.Data.SqlClient` + `SqlBulkCopy`

## Data Processed
- **TotalRecords:** 29818 (total number of records stored in the database.)
- **Duplicates:** 15
- **Errors:** 167 (total number of records that are missing required fields or containing negative values in numeric columns.)

## Workflow

* **Extract:** Stream CSV row-by-row with `CsvHelper`.
* **Validate** (`CabTripValidator`):

  * Require: `tpep_pickup_datetime`, `tpep_dropoff_datetime`, `passenger_count`, `trip_distance`, `PULocationID`, `DOLocationID`, `fare_amount`, `tip_amount`, `store_and_fwd_flag`.
  * `dropoff > pickup`.
  * Numeric fields are **non-negative** (`≥ 0`).
* **Normalize** (`CabTripNormalizerService`):

  * Trim `store_and_fwd_flag`; map `Y`→`Yes`, `N`→`No`; otherwise `null`.
  * Convert `pickup`/`dropoff` from **EST** to **UTC**.
* **Deduplicate:** In-memory `HashSet<(pickupUtc, dropoffUtc, passengerCount)>`.
* **Load:** Batch insert via `SqlBulkCopy` into `dbo.CabTrips`.
* **Outputs:**

  * `sample-cab-duplicates.csv` — original (pre-normalized) duplicate rows.
  * `sample-cab-errors.csv` — original rows that failed validation.


## Project Setup

1. Create DB/table using Sql scripts.
2. Set `appsettings.json` paths/connection.
3. Build & run:

   ```bash
   dotnet build
   dotnet run --project src/TestTask.Console
   ```

## Scaling to Large Files (e.g., 10GB)

* Keep **batching** with `SqlBulkCopy` (use `TableLock`, tune `BatchSize`, increase `BulkCopyTimeout`).
* Consider **streaming directly to `SqlBulkCopy`** (IDataReader) to avoid DataTable overhead.
* Move dedup to SQL staging with `ROW_NUMBER()` if memory becomes a concern.
* Avoid per-row file flushes when writing CSV outputs.

