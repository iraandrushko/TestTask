-- PULocationId with the highest tip_amount on average.
SELECT TOP (1) PULocationID, AVG(tip_amount) AS AverageTip
FROM dbo.CabTrips
GROUP BY PULocationID
ORDER BY AverageTip DESC;

-- Top 100 longest fares in terms of `trip_distance`.
SELECT TOP (100) *
FROM dbo.CabTrips
ORDER BY trip_distance DESC;

-- Top 100 longest fares in terms of time spent traveling.
SELECT TOP (100) *
FROM dbo.CabTrips
ORDER BY DATEDIFF(SECOND, tpep_pickup_datetime, tpep_dropoff_datetime) DESC;

-- Search, where part of the conditions is `PULocationId`.
SELECT Id, tpep_pickup_datetime, tpep_dropoff_datetime, PULocationID
FROM dbo.CabTrips
WHERE PULocationID = @PULocationID;
