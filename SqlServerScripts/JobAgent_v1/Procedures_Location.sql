-- USE Database to inject procedures into.
USE [JobAgentDB]
GO

/*
	## Location ##
*/

DROP PROCEDURE IF EXISTS [CreateLocation]
DROP PROCEDURE IF EXISTS [UpdateLocation]
DROP PROCEDURE IF EXISTS [RemoveLocation]
DROP PROCEDURE IF EXISTS [GetLocationById]
DROP PROCEDURE IF EXISTS [GetAllLocations]
GO

-- Create location
CREATE PROCEDURE [CreateLocation]
	@name varchar(100),
	@description varchar(150)
AS
	-- Check if the location already exists
	IF NOT EXISTS (SELECT [Id] FROM [Location] WHERE [Location].[Name] = @name)
		INSERT INTO [Location] (
			[Name], [Description]
		)
		VALUES (
			@name, @description
		)
GO

-- Update location
CREATE PROCEDURE [UpdateLocation]
	@id int,
	@name varchar(100),
	@description varchar(150)
AS
	-- Check if the location exists
	IF EXISTS (SELECT [Id] FROM [Location] WHERE [Location].[Id] = @id)
		
		/* ## Name ## */
		IF NULLIF(@name, '') IS NULL
			SET @name = (SELECT [Name] FROM [Location] WHERE [Location].[Id] = @id)

		/* ## Description ## */
		IF NULLIF(@description, '') IS NULL
			SET @description = (SELECT [Description] FROM [Location] WHERE [Location].[Id] = @id)

		UPDATE
			[Location]
		SET
			[Name] = @name,
			[Description] = @description
		WHERE
			[Location].[Id] = @id
GO

-- Remove location by id
CREATE PROCEDURE [RemoveLocation]
	@id int
AS
	-- Check if the location exists
	IF EXISTS (SELECT [Id] FROM [Location] WHERE [Location].[Id] = @id)
		-- Delete row.
		DELETE FROM
			[Location]
		WHERE
			[Location].[Id] = @id
GO

-- Get location by id
CREATE PROCEDURE [GetLocationById]
	@id int
AS
	-- Check if exists
	IF EXISTS (SELECT [Id] FROM [Location] WHERE [Location].[Id] = @id)
		SELECT
			[Id],
			[Name],
			[Description]
		FROM
			[Location]
		WHERE
			[Location].[Id] = @id
GO

-- Get all locations
CREATE PROCEDURE [GetAllLocations]
AS
	-- Check if any exists
	IF EXISTS (SELECT [Id] FROM [Location])
		SELECT
			[Id],
			[Name],
			[Description]
		FROM
			[Location]
GO