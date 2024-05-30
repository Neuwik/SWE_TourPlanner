CREATE TABLE TourLog (
    TourLogId INT PRIMARY KEY IDENTITY(1,1),
    TourId INT NOT NULL,
    DateTime DATETIME NOT NULL,
    Comment VARCHAR(255),
    Difficulty INT NOT NULL,
    TotalDistance FLOAT NOT NULL,
    TotalTime FLOAT NOT NULL,
    Rating INT NOT NULL,
    FOREIGN KEY (TourId) REFERENCES Tour(TourId)
);
