USE Soilution_Logs

CREATE TABLE Logs(
    Id int NOT NULL PRIMARY KEY IDENTITY(1,1),
    TimeStamp datetime NOT NULL,
    Level nvarchar(10),
    Message nvarchar(max),
    StackTrace nvarchar(max),
    Exception nvarchar(max),
    Logger nvarchar(255),
    Url nvarchar(255)
);