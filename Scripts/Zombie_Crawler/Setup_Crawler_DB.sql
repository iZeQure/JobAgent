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

INSERT INTO [Crawler] ([Name], [Description]) VALUES
('Zombie Crawler', 'Støvsuger')
GO

INSERT INTO [KeyType] ([Name]) VALUES
('Title_Key'),
('Email_Key'),
('Phone_Number_Key'),
('Description_Key'),
('Location_Key'),
('Registration_Date_Key'),
('Deadline_Date_Key'),
('Category_Key'),
('Specialization_Key')
GO

-- Title Keys
DECLARE @title_key_id INT = 1
INSERT INTO [AlgorithmKeyWords] ([Key], [KeyTypeId]) VALUES
('datatekniker', @title_key_id), 
('it-support', @title_key_id), 
('infrastruktur', @title_key_id), 
('programmering', @title_key_id), 
('programmør', @title_key_id), 
('support', @title_key_id), 
('it support', @title_key_id)
GO

-- Email Keys
DECLARE @email_key_id INT = 2
INSERT INTO [dbo].[AlgorithmKeyWords] ([Key], [KeyTypeId]) VALUES
('a[href*="mailto"]', @email_key_id), 
('a[href^="mailto"]', @email_key_id)
GO

-- Phone Number Keys
DECLARE @phone_number_key_id INT = 3
INSERT INTO [dbo].[AlgorithmKeyWords] ([Key], [KeyTypeId]) VALUES
('td', @phone_number_key_id)
GO

-- Description Keys
DECLARE @description_key_id INT = 4
INSERT INTO [dbo].[AlgorithmKeyWords] ([Key], [KeyTypeId]) VALUES
('description', @description_key_id), 
('area body', @description_key_id), 
('article__body', @description_key_id), 
('article_body', @description_key_id), 
('main span', @description_key_id)
GO

-- Location Keys
DECLARE @location_key_id INT = 5
INSERT INTO [dbo].[AlgorithmKeyWords] ([Key], [KeyTypeId]) VALUES
('adresse', @location_key_id)
GO

-- Registration Date Keys
DECLARE @registration_date_key_id INT = 6
INSERT INTO [dbo].[AlgorithmKeyWords] ([Key], [KeyTypeId]) VALUES
('opslagsdato', @registration_date_key_id)
GO

-- Deadline Date Keys
DECLARE @deadline_date_key_id INT = 7
INSERT INTO [dbo].[AlgorithmKeyWords] ([Key], [KeyTypeId]) VALUES
('ansøgningsfrist', @deadline_date_key_id), 
('ansættelsesdato', @deadline_date_key_id)
GO

-- Category Keys
DECLARE @category_key_id INT = 8
INSERT INTO [dbo].[AlgorithmKeyWords] ([Key], [KeyTypeId]) VALUES
('data-', @category_key_id), 
('kommunikationsuddannelsen', @category_key_id)
GO

-- Specialization Keys
DECLARE @specialization_key_id INT = 9
INSERT INTO [dbo].[AlgorithmKeyWords] ([Key], [KeyTypeId]) VALUES
('programmering', @specialization_key_id)
GO

INSERT INTO [VersionControl] ([CrawlerId], [Major],[Minor],[Patch])
VALUES
(1, 0, 14, 2)
GO