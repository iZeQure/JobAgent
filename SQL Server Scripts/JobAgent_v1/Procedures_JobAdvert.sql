-- USE Database to inject procedures into.
USE [JobAgentDB]
GO

/*
	## Job Advert ##
*/

-- Drop Job Advert Procedures if exists.
DROP PROCEDURE IF EXISTS [CreateJobAdvert]
DROP PROCEDURE IF EXISTS [UpdateJobAdvert]
DROP PROCEDURE IF EXISTS [RemoveJobAdvert]
DROP PROCEDURE IF EXISTS [GetJobAdvertById]
DROP PROCEDURE IF EXISTS [GetAllJobAdverts]

DROP PROCEDURE IF EXISTS [GetAllJobAdvertsForAdmins]
DROP PROCEDURE IF EXISTS [GetJobAdvertDetailsForAdminsById]

DROP PROCEDURE IF EXISTS [GetAllJobAdvertsSortedByCategoryId]
DROP PROCEDURE IF EXISTS [GetAllJobAdvertsSortedBySpecializationId]
GO

-- Create a job advert.
CREATE PROCEDURE [CreateJobAdvert]
	@title varchar(255),
	@email varchar(128),
	@phoneNumber varchar(64),
	@jobDescription varchar(MAX),
	@jobLocation varchar(255),
	@regDate date,
	@deadlineDate date,
	@sourceURL varchar(1000),
	@companyId int, -- FK
	@categoryId int, -- FK
	@specializationId int -- FK
AS
	-- Insert data in the table.
	INSERT INTO
		[JobAdvert] (
			[Title], [Email], [PhoneNumber], [JobDescription], [JobLocation], [JobRegisteredDate], [DeadlineDate], [SourceURL], [CompanyId], [CategoryId], [SpecializationId]
		)
	VALUES (
		@title,
		@email,
		@phoneNumber,
		@jobDescription,
		@jobLocation,
		@regDate,
		@deadlineDate,
		@sourceURL,
		@companyId,
		@categoryId,
		@specializationId
	)		
GO

-- Update Job Advert
CREATE PROCEDURE [UpdateJobAdvert]
	@id int,
	@title varchar(255),
	@email varchar(128),
	@phoneNumber varchar(64),
	@jobDescription varchar(MAX),
	@jobLocation varchar(255),
	@regDate date,
	@deadlineDate Date,
	@sourceURL varchar(1000),
	@companyId int,
	@categoryId int,
	@specializationId int
AS
	/*
		## Check For Null Values. ##
	*/
	-- Title
	IF NULLIF(@title, '') IS NULL
		SET @title = (SELECT [JobAdvert].[Title] FROM [JobAdvert] WHERE [JobAdvert].[Id] = @id)
	-- Email
	IF NULLIF(@email, '') IS NULL
		SET @email = (SELECT [JobAdvert].[Email] FROM [JobAdvert] WHERE [JobAdvert].[Id] = @id)
	-- Phone Number
	IF NULLIF(@phoneNumber, '') IS NULL
		SET @phoneNumber = (SELECT [JobAdvert].[PhoneNumber] FROM [JobAdvert] WHERE [JobAdvert].[Id] = @id)
	-- Job Description
	IF NULLIF(@jobDescription, '') IS NULL
		SET @jobDescription = (SELECT [JobAdvert].[JobDescription] FROM [JobAdvert] WHERE [JobAdvert].[Id] = @id)
	-- Job Location
	IF NULLIF(@jobLocation, '') IS NULL
		SET @jobLocation = (SELECT [JobAdvert].[JobLocation] FROM [JobAdvert] WHERE [JobAdvert].[Id] = @id)
	-- Registration Date
	IF NULLIF(@regDate, '') IS NULL
		SET @regDate = (SELECT [JobAdvert].[JobRegisteredDate] FROM [JobAdvert] WHERE [JobAdvert].[Id] = @id)
	-- Deadline Date
	IF NULLIF(@deadlineDate, '') IS NULL
		SET @deadlineDate = (SELECT [JobAdvert].[DeadlineDate] FROM [JobAdvert] WHERE [JobAdvert].[Id] = @id)
	IF NULLIF(@sourceURL, '') IS NULL
		SET @sourceURL = (SELECT [JobAdvert].[SourceURL] FROM [JobAdvert] WHERE [JobAdvert].[Id] = @id)
	-- Company CVR
	IF NULLIF(@companyId, '') IS NULL
		SET @companyId = (SELECT [JobAdvert].[CompanyId] FROM [JobAdvert] WHERE [JobAdvert].[Id] = @id)	
	-- Job Advert Category Id
	IF NULLIF(@categoryId, '') IS NULL
		SET @categoryId = (SELECT [JobAdvert].[CategoryId] FROM [JobAdvert] WHERE [JobAdvert].[Id] = @id)
	-- Job Advert Category Specialization Id
	IF NULLIF(@specializationId, '') IS NULL
		SET @specializationId = 0

	UPDATE [JobAdvert]
		SET
			[Title] = @title,
			[Email] = @email,
			[PhoneNumber] = @phoneNumber,
			[JobDescription] = @jobDescription,
			[JobLocation] = @jobLocation,
			[JobRegisteredDate] = @regDate,
			[DeadlineDate] = @deadlineDate,
			[SourceURL] = @sourceURL,
			[CompanyId] = @companyId,
			[CategoryId] = @categoryId,
			[SpecializationId] = @specializationId
	WHERE
		[JobAdvert].[Id] = @id			
GO

-- Remove job advert
CREATE PROCEDURE [RemoveJobAdvert]
	-- Job Advert ID Parameter
	@id int
AS
	DELETE FROM 
		[JobAdvert]
	WHERE 
		[JobAdvert].[Id] = @id
GO

-- Get job advert by id
CREATE PROCEDURE [GetJobAdvertById]
	-- Job Advert Id
	@id int
AS 
	SELECT 
		[JobAdvert].[Id],
		[JobAdvert].[Title],
		[JobAdvert].[Email],
		[JobAdvert].[PhoneNumber],
		[JobAdvert].[JobDescription],
		[JobAdvert].[JobLocation],
		[JobAdvert].[JobRegisteredDate],
		[JobAdvert].[DeadlineDate],
		[JobAdvert].[SourceURL],
		[JobAdvert].[CompanyId],
		[Company].[Name] AS 'CompanyName',
		[JobAdvert].[CategoryId],
		[Category].[Name] AS 'CategoryName',
		[JobAdvert].[SpecializationId],
		[Specialization].[Name] AS 'SpecializationName'
	FROM
		[JobAdvert]
	
	INNER JOIN [Company]
	ON [Company].[Id] = [JobAdvert].[CompanyId]

	INNER JOIN [Category]
	ON [Category].[Id] = [JobAdvert].[CategoryId]

	INNER JOIN [Specialization]
	ON [Specialization].[Id] = [JobAdvert].[SpecializationId]

	WHERE
		[JobAdvert].[Id] = @id
GO

-- Return all result in Job Adverts
CREATE PROCEDURE [GetAllJobAdverts]
AS
	SELECT
		[JobAdvert].[Id],
		[JobAdvert].[Title],
		[JobAdvert].[Email],
		[JobAdvert].[PhoneNumber],
		[JobAdvert].[JobDescription],
		[JobAdvert].[JobLocation],
		[JobAdvert].[JobRegisteredDate],
		[JobAdvert].[DeadlineDate],
		[JobAdvert].[SourceURL],
		[JobAdvert].[CompanyId],
		[Company].[Name] AS 'CompanyName',
		[JobAdvert].[CategoryId],
		[Category].[Name] AS 'CategoryName',
		[JobAdvert].[SpecializationId],
		[Specialization].[Name] AS 'SpecializationName'
	FROM
		[JobAdvert]

	INNER JOIN [Company]
	ON [Company].[Id] = [JobAdvert].[CompanyId]

	INNER JOIN [Category]
	ON [Category].[Id] = [JobAdvert].[CategoryId]

	INNER JOIN [Specialization]
	ON [Specialization].[Id] = [JobAdvert].[SpecializationId]
GO

CREATE PROCEDURE [GetAllJobAdvertsForAdmins]
AS
	SELECT
		[JobAdvert].[Id] AS 'JobId',
		[Title],

		[Category].[Name] AS 'Category', 
		[Specialization].[Name] AS 'Specialization', 
	
		[Company].[Name], 
	
		[JobRegisteredDate], 
		[DeadlineDate]
	FROM
		[JobAdvert]
	INNER JOIN
		[Category]
		ON 
			[Category].[Id] = [JobAdvert].[CategoryId]
	INNER JOIN
		[Specialization]
		ON
			[Specialization].[Id] = [JobAdvert].[SpecializationId]
	INNER JOIN
		[Company]
		ON
			[Company].[Id] = [JobAdvert].[CompanyId]
GO

CREATE PROCEDURE [GetJobAdvertDetailsForAdminsById]
	@id int
AS
	-- Get full details of job vancacy.
	SELECT
		[JobAdvert].[Id] AS 'BaseId',
		[Title],
		[Email],
		[PhoneNumber],
		[JobDescription] AS 'Desc',
		[JobLocation] AS 'Loc',
		[JobRegisteredDate] AS 'RegDate',
		[DeadlineDate],
		[SourceURL],

		[CompanyId] AS 'CompanyId',
		[Company].[Name] AS 'CompanyName',

		[JobAdvert].[CategoryId] AS 'CategoryId',
		[Category].[Name] AS 'CategoryName',

		[JobAdvert].[SpecializationId] AS 'SpecializationId',
		[Specialization].[Name] AS 'SpecializationName'
	FROM
		[JobAdvert]
	INNER JOIN
		[Company]
		ON
			[Company].[Id] = [CompanyId]
	INNER JOIN
		[Category]
		ON
			[Category].[Id] = [CategoryId]
	INNER JOIN
		[Specialization]
		ON
			[Specialization].[Id] = [SpecializationId]
	WHERE
		[JobAdvert].[Id] = @id
GO

CREATE PROCEDURE [GetAllJobAdvertsSortedByCategoryId]
	@categoryId int
AS
	BEGIN
		SELECT
			[JobAdvert].[Id], [Title], [Email], [PhoneNumber], [JobDescription], [JobLocation], [JobRegisteredDate], [DeadlineDate], [SourceURL],

			[JobAdvert].[CompanyId], [Company].[Name], [URL],

			[JobAdvert].[CategoryId], [Category].[Name]

		FROM [JobAdvert]

		INNER JOIN [Company] ON [Company].[Id] = [JobAdvert].[CompanyId]
		INNER JOIN [Category] ON [Category].[Id] = [JobAdvert].[CategoryId]

		WHERE [JobAdvert].[CategoryId] = @categoryId
	END
GO

CREATE PROCEDURE [GetAllJobAdvertsSortedBySpecializationId]
	@specialityId int
AS
	BEGIN
		SELECT
			[JobAdvert].[Id], [Title], [Email], [PhoneNumber], [JobDescription], [JobLocation], [JobRegisteredDate], [DeadlineDate], [SourceURL],

			[JobAdvert].[CompanyId], [Company].[Name], [URL],

			[JobAdvert].[CategoryId], [Category].[Name],

			[JobAdvert].[SpecializationId], [Specialization].[Name]

		FROM [JobAdvert]

		INNER JOIN [Company] ON [Company].[Id] = [JobAdvert].[CompanyId]
		INNER JOIN [Category] ON [Category].[Id] = [JobAdvert].[CategoryId]
		INNER JOIN [Specialization] ON [Specialization].[Id] = [JobAdvert].[SpecializationId]

		WHERE [JobAdvert].[SpecializationId] = @specialityId
	END
GO