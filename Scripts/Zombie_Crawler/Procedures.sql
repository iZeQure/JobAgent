USE [ZombieCrawlerDB]
GO

CREATE PROCEDURE [GetKeysByKeyValue] (
	@key_value VARCHAR(50))
AS
	SELECT [Key] FROM [AlgorithmKeyWords]
	INNER JOIN [KeyType] ON [KeyTypeId] = [KeyType].[Id]
	WHERE [Name] = @key_value
GO