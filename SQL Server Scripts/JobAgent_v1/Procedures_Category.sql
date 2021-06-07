-- USE Database to inject procedures into.
USE [JobAgentDB]
GO

/*
	## Job Advert Category- & Specialization ##
*/

-- Drop all Category- & Specialization Procedures if exists.
DROP PROCEDURE IF EXISTS [CreateCategory]
DROP PROCEDURE IF EXISTS [CreateSpecialization]
DROP PROCEDURE IF EXISTS [UpdateCategory]
DROP PROCEDURE IF EXISTS [UpdateSpecialization]
DROP PROCEDURE IF EXISTS [RemoveCategory]
DROP PROCEDURE IF EXISTS [RemoveSpecialization]
DROP PROCEDURE IF EXISTS [GetCategoryById]
DROP PROCEDURE IF EXISTS [GetSpecializationById]
DROP PROCEDURE IF EXISTS [GetAllCategories]
DROP PROCEDURE IF EXISTS [GetAllSpecializations]
DROP PROCEDURE IF EXISTS [GetAllCategoriesWithSpecialization]
GO

CREATE PROCEDURE [CreateCategory]
	@name varchar(150)
AS
	IF NOT EXISTS (SELECT [Name] FROM [Category] WHERE [Category].[Name] = @name)
		-- Create category
		INSERT INTO
			[Category]
				([Name])
		VALUES
			(@name)
		
GO

CREATE PROCEDURE [CreateSpecialization]
	@id int,
	@name varchar(100)
AS
	IF NOT EXISTS (SELECT [Name] FROM [Specialization] WHERE [Specialization].[Name] = @name)
		INSERT INTO
			[Specialization]
				([Name], [CategoryId])
		VALUES
			(@name, @id)		
GO

CREATE PROCEDURE [UpdateCategory]
	@id int,
	@name varchar(150)
AS
	UPDATE
		[Category]
	SET
		[Name] = @name
	WHERE
		[Category].[Id] = @id
GO

CREATE PROCEDURE [UpdateSpecialization]
	@categoryId int,
	@specializationId int,
	@name varchar(100)
AS
	UPDATE
		[Specialization]
	SET
		[Name] = @name
	WHERE
		[Specialization].[Id] = @specializationId AND
		[Specialization].[CategoryId] = @categoryId
GO

CREATE PROCEDURE [RemoveCategory]
	@id int
AS
	DELETE FROM [Specialization] WHERE [Specialization].[CategoryId] = @id
	DELETE FROM [Category] WHERE [Category].[Id] = @id
GO

CREATE PROCEDURE [RemoveSpecialization]
	@id int
AS
	-- Delete speciality from category.
	DELETE FROM [Specialization]
	WHERE
		[Specialization].[Id] = @id
GO

CREATE PROCEDURE [GetCategoryById]
	@id int
AS
	SELECT
		[Category].[Id],
		[Category].[Name]
	FROM
		[Category]
	WHERE
		[Category].[Id] = @id
GO

CREATE PROCEDURE [GetSpecializationById]
	@id int
AS
	SELECT TOP(1)
		[Specialization].[Id],
		[Specialization].[Name]
	FROM
		[Specialization]
	WHERE 
		[Specialization].[Id] = @id
GO

CREATE PROCEDURE [GetAllCategories]
AS
	-- Check for any content
	IF EXISTS (SELECT [Id] FROM [Category])
		SELECT
			[Id],
			[Name]
		FROM
			[Category]
		WHERE
			[Name] IS NOT NULL
GO

CREATE PROCEDURE [GetAllSpecializations]
AS
	-- Check for any content
	IF EXISTS (SELECT [Id] FROM [Specialization])
		SELECT
			[Id],
			[Name],
			[CategoryId]
		FROM
			[Specialization]
		WHERE
			[Specialization].[Name] IS NOT NULL
GO

CREATE PROCEDURE [GetAllCategoriesWithSpecialization]
AS
	IF EXISTS (SELECT [Id] FROM [Category])
		IF EXISTS (SELECT [Id] FROM [Specialization])			
			SELECT
				[Category].[Id],
				[Category].[Name] AS 'CategoryName',

				[Specialization].[Id] AS 'SpecId',
				[Specialization].[Name] AS 'SpecializationName',
				[Specialization].[CategoryId]
			FROM
				[Category]
			LEFT OUTER JOIN 
				[Specialization]
			ON 
				[Category].[Id] = [Specialization].[CategoryId]
			WHERE 
				[Category].[Name] IS NOT NULL
GO