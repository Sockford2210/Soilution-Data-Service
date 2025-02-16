USE master
CREATE LOGIN LoggingUser
WITH PASSWORD = '#LoggingPassword1234';

USE Soilution_Logs
CREATE USER LoggingUser FOR LOGIN LoggingUser

ALTER ROLE db_datareader ADD MEMBER LoggingUser
ALTER ROLE db_datawriter ADD MEMBER LoggingUser

GRANT SELECT, INSERT
 ON [Soilution_Logs].[dbo].[Logs]
TO LoggingUser 

-- USE master
-- GO 
-- SELECT spid, blocked  AS BlockedBy, loginame  AS LogInName, login_time,
-- last_batch, status
-- FROM   sys.sysprocesses
-- WHERE loginame = 'LoggingLogin'   --Change the loginID you are trying to delete

-- DROP LOGIN LoggingLogin