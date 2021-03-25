USE [master]
GO

DECLARE @kill varchar(8000) = ''
SELECT @kill = @kill + 'kill ' + CONVERT(varchar(5), session_id) + ';'
FROM sys.dm_exec_sessions
WHERE database_id = DB_ID('JobAgentLogDB')

EXEC(@kill)
GO

DROP DATABASE IF EXISTS [JobAgentLogDB]
GO

CREATE DATABASE [JobAgentLogDB]
GO

USE [JobAgentLogDB]
GO

DROP TABLE IF EXISTS [Log]
DROP TABLE IF EXISTS [LogSeverity]
DROP TABLE IF EXISTS [LogType]

DROP PROCEDURE IF EXISTS [JA.log.spCreateLog]
DROP PROCEDURE IF EXISTS [JA.log.spGetLogInformation]
GO

CREATE TABLE [Log] (
[Id] int not null identity(1,1),
[Name] varchar(20) not null,
[Message] varchar(100) not null,
[RegisteredDateTime] datetime default GETDATE(),
[LogSeverityId] int not null,
[LogTypeId] int not null)
GO

CREATE TABLE [LogSeverity] (
[Id] int not null identity(1,1),
[Name] varchar(50) not null)
GO

CREATE TABLE [LogType] (
[Id] int not null identity(1,1),
[Name] varchar(30) not null)
GO

ALTER TABLE [LogSeverity]
ADD PRIMARY KEY ([Id])
GO

ALTER TABLE [LogType]
ADD PRIMARY KEY ([Id])
GO

ALTER TABLE [Log]
ADD PRIMARY KEY ([Id]),
	FOREIGN KEY ([LogSeverityId]) REFERENCES [LogSeverity] ([Id]) ON UPDATE CASCADE ON DELETE CASCADE,
	FOREIGN KEY ([LogTypeId]) REFERENCES [LogType] ([Id]) ON UPDATE CASCADE ON DELETE CASCADE
GO

INSERT INTO [LogSeverity] ([Name])
VALUES
('Verbose'),
('Debug'),
('Info'),
('Warning'),
('Error'),
('Critical')
GO

INSERT INTO [LogType] ([Name])
VALUES
('General'),
('System')
GO

CREATE PROCEDURE [JA.log.spCreateLog] (
	@name varchar(20),
	@message varchar(100),
	@logSeverityId int,
	@logTypeId int)
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'LogCreate';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Creating a log';

	BEGIN TRY
		INSERT INTO [Log] ([Name], [Message], [LogSeverityId], [LogTypeId])
		VALUES
		(@name, @message, @logSeverityId, @logTypeId)

		COMMIT TRANSACTION @TranName
	END TRY
	BEGIN CATCH
		INSERT INTO [Log] ([Name], [Message], [LogSeverityId], [LogTypeId])
		VALUES
		(@TranName, 'Failed Creating Log', 4, 1)

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

CREATE PROCEDURE [JA.log.spGetLogInformation] (
	@sortedBySeverity int = 0)
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'LogGetInformation';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Displaying log information';

	BEGIN TRY
		IF @sortedBySeverity = 0
			SELECT
				[dbo].[Log].[Id] AS 'Log ID',
				[dbo].[Log].[Name] AS 'Log Name',
				[dbo].[Log].[Message] AS 'Log Message',
				[dbo].[Log].[RegisteredDateTime] AS 'Registration Date',

				[dbo].[LogSeverity].[Name] AS 'Log Severity',
				[dbo].[LogType].[Name] AS 'Log Type'
			FROM [dbo].[Log]
				INNER JOIN [dbo].[LogSeverity] ON [dbo].[LogSeverity].[Id] = [dbo].[Log].[LogSeverityId]
				INNER JOIN [dbo].[LogType] ON [dbo].[LogType].[Id] = [dbo].[Log].[LogTypeId]
			ORDER BY [dbo].[Log].[Id] DESC
		ELSE
			SELECT
				[dbo].[Log].[Id] AS 'Log ID',
				[dbo].[Log].[Name] AS 'Log Name',
				[dbo].[Log].[Message] AS 'Log Message',
				[dbo].[Log].[RegisteredDateTime] AS 'Registration Date',

				[dbo].[LogSeverity].[Name] AS 'Log Severity',
				[dbo].[LogType].[Name] AS 'Log Type'
			FROM [dbo].[Log]
				INNER JOIN [dbo].[LogSeverity] ON [dbo].[LogSeverity].[Id] = [dbo].[Log].[LogSeverityId]
				INNER JOIN [dbo].[LogType] ON [dbo].[LogType].[Id] = [dbo].[Log].[LogTypeId]
			WHERE [dbo].[Log].[LogSeverityId] = @sortedBySeverity
			ORDER BY [dbo].[Log].[Id] DESC

		COMMIT TRANSACTION @TranName
	END TRY
	BEGIN CATCH
		EXEC [dbo].[JA.log.spCreateLog] @TranName, 'Could not get log information', 5, 1
		
		ROLLBACK TRANSACTION @TranName
	END CATCH
GO