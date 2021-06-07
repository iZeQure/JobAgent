-- USE Database to inject procedures into.
USE [JobAgentDB]
GO

/*
	## Source Link ##
*/

DROP PROCEDURE IF EXISTS [CreateSourceLink]
DROP PROCEDURE IF EXISTS [UpdateSourceLink]
DROP PROCEDURE IF EXISTS [RemoveSourceLink]
DROP PROCEDURE IF EXISTS [GetSourceLinkById]
DROP PROCEDURE IF EXISTS [GetAllSourceLinks]
GO

CREATE PROCEDURE [CreateSourceLink]
	@companyId int,
	@link varchar(500),
	@output int OUTPUT
AS
	IF NOT EXISTS (SELECT * FROM [SourceLink] WHERE [SourceLink].[CompanyId] = @companyId AND [SourceLink].[Link] = @link)
		BEGIN
			SET @output = 1

			INSERT INTO
				[SourceLink] 
				([CompanyId], [Link])
			VALUES
				(@companyId, @link)
		END
	ELSE
		BEGIN
			SET @output = 0
		END
GO

CREATE PROCEDURE [UpdateSourceLink]
	@id int,
	@companyId int,
	@link varchar(500),
	@output int OUTPUT
AS
	IF EXISTS (SELECT * FROM [SourceLink] WHERE [SourceLink].[Id] = @id)
		BEGIN
			SET @output = 1

			UPDATE
				[SourceLink]
			SET
				[CompanyId] = @companyId,
				[Link] = @link
			WHERE
				[Id] = @id
		END
	ELSE
		BEGIN
			SET @output = 0
		END
GO

CREATE PROCEDURE [RemoveSourceLink]
	@id int,
	@output int OUTPUT
AS
	IF EXISTS (SELECT * FROM [SourceLink] WHERE [SourceLink].[Id] = @id)
		BEGIN
			SET @output = 1

			DELETE FROM 
				[SourceLink]
			WHERE
				[Id] = @id
		END
	ELSE
		BEGIN
			SET @output = 0
		END
GO

CREATE PROCEDURE [GetSourceLinkById]
	@id int,
	@output int OUTPUT
AS
	IF EXISTS (SELECT * FROM [SourceLink] WHERE [SourceLink].[Id] = @id)
		BEGIN
			SET @output = 1

			SELECT
				[SourceLink].[Id] AS 'Id',
				[CompanyId],
				[Company].[Name],
				[Link]
			FROM
				[SourceLink]
			INNER JOIN
				[Company]
				ON [CompanyId] = [Company].[Id]
			WHERE
				[SourceLink].[Id] = @id
		END
	ELSE
		BEGIN
			SET @output = 0
		END
GO

CREATE PROCEDURE [GetAllSourceLinks]
	@output int OUTPUT
AS
	IF EXISTS (SELECT * FROM [SourceLink])
		BEGIN
			SET @output = 1

			SELECT
				[SourceLink].[Id] AS 'Id',
				[CompanyId],
				[Company].[Name],
				[Link]
			FROM
				[SourceLink]
			INNER JOIN
				[Company]
				ON [CompanyId] = [Company].[Id]
		END
	ELSE
		BEGIN
			SET @output = 0
		END
GO