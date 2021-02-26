USE [master]
GO

DECLARE @kill varchar(8000) = ''
SELECT @kill = @kill + 'kill ' + CONVERT(varchar(5), session_id) + ';'
FROM sys.dm_exec_sessions
WHERE database_id = DB_ID('ZombieCrawlerDB')

EXEC(@kill)
GO

DROP DATABASE IF EXISTS [ZombieCrawlerDB]
GO

CREATE DATABASE [ZombieCrawlerDB]
GO

USE [ZombieCrawlerDB]
GO

DROP TABLE IF EXISTS [Crawler]
DROP TABLE IF EXISTS [VersionControl]
DROP TABLE IF EXISTS [AlgorithmKeyWords]
DROP TABLE IF EXISTS [KeyType]
GO

CREATE TABLE [Crawler](
[Id] INT NOT NULL IDENTITY(1,1),
[Name] VARCHAR(50) NOT NULL,
[Description] VARCHAR(100) NOT NULL
)
GO

CREATE TABLE [VersionControl] (
[Id] INT NOT NULL IDENTITY(1,1),
[Major] INT NOT NULL,
[Minor] INT NOT NULL,
[Patch] INT NOT NULL,
[CrawlerId] INT NOT NULL
)
GO

CREATE TABLE [AlgorithmKeyWords] (
[Id] INT NOT NULL IDENTITY(1,1),
[Key] VARCHAR(50) NOT NULL,
[KeyTypeId] INT NOT NULL
)
GO

CREATE TABLE [KeyType] (
[Id] INT NOT NULL IDENTITY(1,1),
[Name] VARCHAR(50) NOT NULL
)
GO

ALTER TABLE [Crawler]
	ADD PRIMARY KEY ([Id])
GO

ALTER TABLE [KeyType]
	ADD PRIMARY KEY ([Id])
GO

ALTER TABLE [VersionControl]
	ADD PRIMARY KEY ([Id]),
		FOREIGN KEY ([CrawlerId]) REFERENCES [Crawler]([Id])
GO

ALTER TABLE [AlgorithmKeyWords]
	ADD PRIMARY KEY ([Id]),
		FOREIGN KEY ([KeyTypeId]) REFERENCES [KeyType]([Id])
GO