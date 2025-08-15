CREATE DATABASE [CabDB]
GO
CREATE TABLE [dbo].[CabTrips](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[tpep_pickup_datetime] [datetime] NULL,
	[tpep_dropoff_datetime] [datetime] NULL,
	[passenger_count] [int] NULL,
	[trip_distance] [float] NULL,
	[store_and_fwd_flag] [varchar](3) NULL,
	[PULocationID] [int] NULL,
	[DOLocationID] [int] NULL,
	[fare_amount] [decimal](10, 2) NULL,
	[tip_amount] [decimal](10, 2) NULL,
	PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	) ON [PRIMARY]
) ON [PRIMARY]
GO

/******Index [idx_pickup_datetime] ******/
CREATE NONCLUSTERED INDEX [idx_pickup_datetime] ON [dbo].[CabTrips]
(
	[tpep_pickup_datetime] ASC,
	[tpep_dropoff_datetime] ASC
) ON [PRIMARY]
GO

/****** Index [idx_PULocation_tip] ******/
CREATE NONCLUSTERED INDEX [idx_PULocation_tip] ON [dbo].[CabTrips]
(
	[PULocationID] ASC,
	[tip_amount] ASC
) ON [PRIMARY]
GO

/****** Index [idx_trip_distance] ******/
CREATE NONCLUSTERED INDEX [idx_trip_distance] ON [dbo].[CabTrips]
(
	[trip_distance] DESC
) ON [PRIMARY]
