USE [ZombieCrawlerDB]
GO

CREATE PROCEDURE [GetKeysByKeyValue] (
	@key_value VARCHAR(50))
AS
	SELECT [Key] FROM [AlgorithmKeyWords]
	INNER JOIN [KeyType] ON [KeyTypeId] = [KeyType].[Id]
	WHERE [Name] = @key_value
GO

CREATE PROCEDURE [GetInitializationInformation]
AS
	SELECT [Name], [Description], 
	CONCAT([VersionControl].[Major],'.',[VersionControl].[Minor],'.',[VersionControl].[Patch]) AS VERSION
	FROM [Crawler]
		INNER JOIN [VersionControl]
			ON [Crawler].[Id] = [VersionControl].[CrawlerId]
	WHERE
		[Name] LIKE '%Zombie%';

GO