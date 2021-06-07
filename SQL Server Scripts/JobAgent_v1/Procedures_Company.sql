-- USE Database to inject procedures into.
USE [JobAgentDB]
GO

/*
	## Company ##
*/

-- Drop Company Procedures if exists.
DROP PROCEDURE IF EXISTS [CreateCompany]
DROP PROCEDURE IF EXISTS [UpdateCompany]
DROP PROCEDURE IF EXISTS [RemoveCompany]
DROP PROCEDURE IF EXISTS [GetCompanyById]
DROP PROCEDURE IF EXISTS [GetAllCompanies]
GO

-- Create a company if it doesn't already exist.
CREATE PROCEDURE [CreateCompany]
	@cvr int,
	@name varchar(200),
	@url varchar(1000)
AS
	IF NOT EXISTS (SELECT * FROM [Company] WHERE [Company].[CVR] = @cvr)
		INSERT INTO
			[Company]
			(
				[CVR], [Name], [URL]
			)
		VALUES
			(
				@cvr, @name, @url
			)		
GO

-- Update existing company
CREATE PROCEDURe [UpdateCompany]
	@id int,
	@cvr int,
	@name varchar(200),
	@url varchar(1000)
AS
	-- Check if the company exists
	IF EXISTS
		(SELECT * FROM [Company] WHERE [Company].[Id] = @id)
			UPDATE [Company]
				SET
					[CVR] = @cvr,
					[Name] = @name,
					[URL] = @url
				WHERE
					[Company].[Id] = @id
GO

-- Remove company and all it's references to other tables.
CREATE PROCEDURE [RemoveCompany]
	@id int
AS
	-- Remove Source Links
	IF EXISTS
		(SELECT * FROM [SourceLink] WHERE [SourceLink].[CompanyId] = @id)
			DELETE FROM
				[SourceLink]
			WHERE
				[SourceLink].[CompanyId] = @id
	-- Remove Contracts
	IF EXISTS 
		(SELECT * FROM [Contract] WHERE [Contract].[CompanyId] = @id)
			DELETE FROM 
				[Contract]
			WHERE
				[Contract].[CompanyId] = @id
	-- Remove Job Vacancies
	IF EXISTS
		(SELECT * FROM [JobAdvert] WHERE [JobAdvert].[CompanyId] = @id)
			DELETE FROM
				[JobAdvert]
			WHERE
				[JobAdvert].[CompanyId] = @id
	-- Remove the Company
	IF EXISTS
		(SELECT * FROM [Company] WHERE [Company].[Id] = @id)
			DELETE FROM 
				[Company]
			WHERE
				[Company].[Id] = @id
GO

-- Get company by id
CREATE PROCEDURE [GetCompanyById]
	@id int
AS
	IF EXISTS
		(SELECT * FROM [Company] WHERE [Company].[id] = @id)
			SELECT
				[Company].[Id],
				[Company].[CVR],
				[Company].[Name],
				[Company].[URL]
			FROM
				[Company]
			WHERE
				[Company].[Id] = @id
	ELSE
		RETURN
GO

-- Returns all results in the Company Table
CREATE PROCEDURE [GetAllCompanies]
AS
	SELECT
		[Company].[Id],
		[Company].[CVR],
		[Company].[Name],
		[Company].[URL]
	FROM
		[Company]
GO