-- USE Database to inject procedures into.
USE [JobAgentDB]
GO

/*
	## Consultant Area ##
*/

DROP PROCEDURE IF EXISTS [CreateConsultantArea]
DROP PROCEDURE IF EXISTS [UpdateConsultantArea]
DROP PROCEDURE IF EXISTS [RemoveConsultantArea]
DROP PROCEDURE IF EXISTS [GetConsultantAreaById]
DROP PROCEDURE IF EXISTS [GetAllConsultantAreas]
GO

-- Create consultant area
CREATE PROCEDURE [CreateConsultantArea]
	@name varchar(100),
	@output int output
AS
	-- Check if the consultant area already exists
	IF EXISTS (SELECT [Id] FROM [ConsultantArea] WHERE [ConsultantArea].[Name] = @name)
		-- Conflict Error
		SET @output = 409
	ELSE
		INSERT INTO [ConsultantArea] (
			[Name]
		)
		VALUES (
			@name
		)

	RETURN @output
GO

-- Update consultant area
CREATE PROCEDURE [UpdateConsultantArea]
	@id int,
	@name varchar(100)
AS
	-- Check if the consultant area exists
	IF EXISTS (SELECT [Id] FROM [ConsultantArea] WHERE [ConsultantArea].[Id] = @id)
		
		/* ## Name ## */
		IF NULLIF(@name, '') IS NULL
			SET @name = (SELECT [Name] FROM [ConsultantArea] WHERE [ConsultantArea].[Id] = @id)

		UPDATE
			[ConsultantArea]
		SET 
			[Name] = @name
		WHERE
			[ConsultantArea].[Id] = @id
GO

-- Remove consultant area by id
CREATE PROCEDURE [RemoveConsultantArea]
	@id int,
	@output int output
AS
	-- Check if the consultant area exists
	IF EXISTS (SELECT [Id] FROM [ConsultantArea] WHERE [ConsultantArea].[Id] = @id)
		-- Delete row.
		DELETE FROM
			[ConsultantArea]
		WHERE
			[ConsultantArea].[Id] = @id
	ELSE
		-- Return not found code
		SET @output = 404

	RETURN @output
GO

-- Get consultant area by id
CREATE PROCEDURE [GetConsultantAreaById]
	@id int,
	@output int output
AS
	-- Check if exists
	IF EXISTS (SELECT [Id] FROM [ConsultantArea] WHERE [ConsultantArea].[Id] = @id)
		SELECT
			[Id],
			[Name]
		FROM
			[ConsultantArea]
		WHERE
			[ConsultantArea].[Id] = @id
	ELSE
		-- Return not found code
		SET @output = 404

	RETURN @output
GO

-- Get all consultant areas
CREATE PROCEDURE [GetAllConsultantAreas]
AS
	-- Check if any exists
	IF EXISTS (SELECT [Id] FROM [ConsultantArea])
		SELECT
			[Id],
			[Name]
		FROM
			[ConsultantArea]
GO