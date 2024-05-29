CREATE TABLE TourLog (
    TourLogId INT PRIMARY KEY IDENTITY(1,1),
    TourId INT NOT NULL,
    DateTime DATETIME NOT NULL,
    Comment VARCHAR(255),
    Difficulty NVARCHAR(50) NOT NULL,
    TotalDistance FLOAT NOT NULL,
    TotalTime FLOAT NOT NULL,
    Rating NVARCHAR(50) NOT NULL,
    CONSTRAINT CHK_Difficulty CHECK (Difficulty IN ('easy', 'medium', 'hard')),
    CONSTRAINT CHK_Rating CHECK (Rating IN ('ZeroStars', 'OneStars', 'TwoStars', 'ThreeStars', 'FourStars', 'FiveStars')),
    FOREIGN KEY (TourId) REFERENCES Tour(TourId)
);
