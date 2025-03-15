USE Soilution_Logs

CREATE TABLE Logs(
    Id int NOT NULL PRIMARY KEY IDENTITY(1,1),
    TimeStamp DATETIME NOT NULL,
    Level NVARCHAR(10),
    Message NVARCHAR(max),
    StackTrace NVARCHAR(max),
    Exception NVARCHAR(max),
    Logger NVARCHAR(255),
    Url NVARCHAR(255)
);