USE Soilution_Data

CREATE TABLE DataHubs (
	Id INT IDENTITY(1,1),
	Name VARCHAR(255),
	DateCreated DATETIME NOT NULL,
	PRIMARY KEY (Id)
);

CREATE TABLE AirQualityDevice (
	Id INT IDENTITY(1,1),
	HubId INT NOT NULL,
	DateCreated DATETIME NOT NULL,
	PRIMARY KEY (Id),
	FOREIGN KEY (HubId) REFERENCES DataHubs(Id)
);

CREATE TABLE AirQualityDataReadings (
    Id INT IDENTITY(1,1),
    DeviceId INT NOT NULL,
	Timestamp DATETIME NOT NULL,
    HumidityPercentage DECIMAL(8,6),
	TemperatureCelcius DECIMAL(8,6),
	CO2PPM DECIMAL(10,6),
	PRIMARY KEY (Id),
	FOREIGN KEY (DeviceId) REFERENCES AirQualityDevice(Id)
);

-- CREATE TABLE SoilQualityDevice (
-- 	Id INT IDENTITY(1,1),
-- 	HubId INT NOT NULL,
-- 	DateCreated DATETIME NOT NULL,
-- 	PRIMARY KEY (Id),
-- 	FOREIGN KEY (HubId) REFERENCES DataHubs(Id)
-- );

-- CREATE TABLE SoilDataReadings (
--     Id INT IDENTITY(1,1),
--     DeviceId INT NOT NULL,
-- 	Timestamp DATETIME NOT NULL,
--     MoisturePercentage INT,
--     PHLevel INT,
-- 	TemperatureCelcius INT,
-- 	SunlightLumens INT
-- 	PRIMARY KEY (Id),
-- 	FOREIGN KEY (DeviceId) REFERENCES SoilQualityDevice(Id)
-- );
