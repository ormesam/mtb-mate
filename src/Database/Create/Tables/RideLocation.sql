﻿CREATE TABLE [RideLocation] (
    [RideLocationId] int NOT NULL IDENTITY(1,1) PRIMARY KEY,

    [RideId] int NOT NULL CONSTRAINT [FK_RideLocation_Ride] REFERENCES [Ride],

    [Timestamp] datetime NOT NULL,
    [Latitude] float NOT NULL,
    [Longitude] float NOT NULL,
    [AccuracyInMetres] float NOT NULL,
    [Mph] float NOT NULL,
    [Altitude] float NOT NULL,

    [IsRemoved] bit NOT NULL,
    [RemovalReason] nvarchar(255) NULL,
)