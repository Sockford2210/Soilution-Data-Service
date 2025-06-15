USE master
CREATE LOGIN DataAPIReadingsUser
WITH PASSWORD = '#DataPassword1234';

USE Soilution_Data
CREATE USER DataAPIReadingsUser FOR LOGIN DataAPIReadingsUser

ALTER ROLE db_datareader ADD MEMBER DataAPIReadingsUser
ALTER ROLE db_datawriter ADD MEMBER DataAPIReadingsUser

GO

USE Soilution_Data
GRANT SELECT, INSERT
ON AirQualityDataReadings, AirQualityDevice, SoilDataReadings, SoilQualityDevice, DataHubs
TO DataAPIReadingsUser 

--EXECUTE AS USER = 'DataAPIReadingsUser';
--GO
--USE Soilution_Data
--GO
--SELECT * FROM fn_my_permissions('Readings', 'OBJECT')
--GO

--SELECT CURRENT_USER

-- USE master
-- GO 
-- SELECT spid, blocked  AS BlockedBy, loginame  AS LogInName, login_time,
-- last_batch, status
-- FROM   sys.sysprocesses
-- WHERE loginame = 'DataAPIReadingsLogin'   --Change the loginID you are trying to delete

-- DROP LOGIN DataAPIReadingsLogin