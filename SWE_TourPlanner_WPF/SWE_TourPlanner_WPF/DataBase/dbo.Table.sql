CREATE TABLE [dbo].[Tour] (
    [TourId] INT PRIMARY KEY IDENTITY(1,1),
    [Name] NVARCHAR(255) NOT NULL,
    [Description] NVARCHAR(255),
    [FromLocation] NVARCHAR(255) NOT NULL,
    [ToLocation] NVARCHAR(255) NOT NULL,
    [TransportType] NVARCHAR(50) NOT NULL,
    [Distance] INT NOT NULL,
    [Time] INT NOT NULL,
    [RouteInformation] NVARCHAR(255),
    [ImagePath] NVARCHAR(255),
    CONSTRAINT CHK_TransportType CHECK (TransportType IN ('Car', 'Bike', 'Foot'))
)
