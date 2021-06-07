-- USE Database to inject procedures into.
USE [JobAgentDB]
GO

/*
	## User ##
*/

-- Repository Procedures..
DROP PROCEDURE IF EXISTS [CreateUser]
DROP PROCEDURE IF EXISTS [UpdateUser]
DROP PROCEDURE IF EXISTS [RemoveUser]
DROP PROCEDURE IF EXISTS [GetUserById]
DROP PROCEDURE IF EXISTS [GetAllUsers]

-- Validation & Login
DROP PROCEDURE IF EXISTS [GetUserByEmail]
DROP PROCEDURE IF EXISTS [GetUserSaltByEmail]
DROP PROCEDURE IF EXISTS [GetUserByAccessToken]
DROP PROCEDURE IF EXISTS [ValidatePassword]
DROP PROCEDURE IF EXISTS [CheckUserExists]
DROP PROCEDURE IF EXISTS [UserLogin]
DROP PROCEDURE IF EXISTS [UserRegistration]

-- Authorization
DROP PROCEDURE IF EXISTS [UpdateUserPassword]
GO

-- Create User if not already exists
CREATE PROCEDURE [CreateUser]
	@firstName varchar(128),
	@lastName varchar(128),
	@email varchar(255),
	@password varchar(255),
	@salt varchar(128),
	@accessToken varchar(8000),
	@consultantAreaId int,
	@locationId int
AS
	-- Check if a user exists with the current email.
	IF NOT EXISTS (SELECT [Email] FROM [dbo].[User] WHERE [User].[Email] = @Email)
		-- Create a user.
		INSERT INTO [User] (
			[FirstName], [LastName], [Email], [Password], [Salt], [AccessToken], [ConsultantAreaId], [LocationId]
		)
		VALUES (
			@firstName, @lastName, @email, @password, @salt, @accessToken, @consultantAreaId, @locationId
		)
GO

-- Update User if changes happened
CREATE PROCEDURE [UpdateUser]
	@id int,
	@firstName varchar(128),
	@lastName varchar(128),
	@email varchar(128),
	@consultantAreaId int,
	@locationId int
AS
	IF EXISTS (SELECT [Id] FROM [User] WHERE [User].[Id] = @id)
		-- Check if paramters contains null.
		/*	## First Name ## */
		IF NULLIF(@firstName, '') IS NULL
			SET @firstName = (SELECT [FirstName] FROM [User] WHERE [User].[Id] = @id)

		/*	## Last Name ##	*/
		IF NULLIF(@lastName, '') IS NULL
			SET @lastName = (SELECT [LastName] FROM [User] WHERE [User].[Id] = @id)

		/*	## Email ##	*/
		IF NULLIF(@email, '') IS NULL
			SET @email = (SELECT [Email] FROM [User] WHERE [User].[Id] = @id)

		/*	## Consultant Area ##	*/
		IF NULLIF(@consultantAreaId, '') IS NULL
			SET @consultantAreaId = (SELECT [ConsultantAreaId] FROM [User] WHERE [User].[Id] = @id)

		/*	## Location ##	*/
		IF NULLIF(@locationId, '') IS NULL
			SET @locationId = (SELECT [LocationId] FROM [User] WHERE [User].[Id] = @id)

		-- Update user fields
		UPDATE [User]
			SET 
				[FirstName] = @firstName,
				[LastName] = @lastName,
				[Email] = @email,
				[ConsultantAreaId] = @consultantAreaId,
				[LocationId] = @locationId
			WHERE
				[User].[Id] = @id
GO

-- Remove user by id if exists
CREATE PROCEDURE [RemoveUser]
	@id int
AS
	IF EXISTS (SELECT [Id] FROM [User] WHERE [User].[Id] = @id)
		DELETE FROM [User]
		WHERE
			[User].[Id] = @id
GO

-- Get user by id if exists
CREATE PROCEDURE [GetUserById]
	@id int
AS
	-- Check if user exist
	IF EXISTS (SELECT [Id] FROM [User] WHERE [User].[Id] = @id)
		SELECT
			-- User information
			[User].[Id],
			[User].[FirstName],
			[User].[LastName],
			[User].[Email],
			-- Consultant information
			[ConsultantArea].[Name] AS 'ConsultantAreaName',
			-- Location information
			[Location].[Name] AS 'LocationName',
			[Location].[Description] AS 'LocationDesc'
		FROM
			[User]
		LEFT JOIN [ConsultantArea]
			ON [User].[ConsultantAreaId] = [ConsultantArea].[Id]
		LEFT JOIN [Location]
			ON [User].[LocationId] = [Location].[Id]
		WHERE
			[User].[Id] = @id
GO

-- Get all users
CREATE PROCEDURE [GetAllUsers]
AS
	IF EXISTS (SELECT [Id] FROM [User])
		SELECT
			-- User information
			[User].[Id],
			[User].[FirstName],
			[User].[LastName],
			[User].[Email],
			-- Consultant information
			[ConsultantArea].[Name] AS 'ConsultantAreaName',
			-- Location information
			[Location].[Name] AS 'LocationName',
			[Location].[Description] AS 'LocationDesc'
		FROM
			[User]
		LEFT JOIN [ConsultantArea]
			ON [User].[ConsultantAreaId] = [ConsultantArea].[Id]
		LEFT JOIN [Location]
			ON [User].[LocationId] = [Location].[Id]
GO

-- Check if email exists, return 1 if true, otherwise 0
CREATE PROCEDURE [CheckUserExists]
	@email varchar(255)
AS
	-- Check if the email exists
	IF EXISTS (SELECT [Email] FROM [User] WHERE [User].[Email] = @email)
		RETURN 1
	ELSE
		RETURN 0
GO

-- Get User salt by email
CREATE PROCEDURE [GetUserSaltByEmail]
	@email varchar(255)
AS
	SELECT 
		[Salt]
	FROM
		[User]
	WHERE
		[User].[Email] = @email
GO

-- Validate password
CREATE PROCEDURE [ValidatePassword]
	@secret varchar(255)
AS
	IF EXISTS (SELECT [Password] FROM [User] WHERE [Password] = @secret)
		RETURN 1
	ELSE
		RETURN 0
GO

-- User login, return user id
CREATE PROCEDURE [UserLogin]
	@email varchar(255),
	@secret varchar(255)
AS
	SELECT
		-- User Information
		[User].[Id] AS 'UserId',
		[FirstName],
		[LastName],
		[Email],
		[AccessToken],

		-- Consultant Information
		[ConsultantArea].[Id] AS 'ConsultantAreaId',
		[ConsultantArea].[Name] AS 'ConsultantAreaName',

		-- Location Information
		[Location].[Id] AS 'LocationId',
		[Location].[Name] AS 'LocationName'
	FROM
		[User]
	LEFT JOIN [ConsultantArea]
	ON [ConsultantArea].[Id] = [User].[ConsultantAreaId]
	LEFT JOIN [Location]
	ON [Location].[Id] = [User].[LocationId]
	WHERE
		[User].[Email] = @email AND
		[User].[Password] = @secret
GO

-- Get user by email
CREATE PROCEDURE [GetUserByEmail]
	@email varchar(255)
AS
	SELECT
		[User].[Id] AS 'UserId',
		[FirstName],
		[LastName],
		[Email],

		[ConsultantArea].[Id] AS 'ConsultantAreaId',
		[ConsultantArea].[Name] AS 'ConsultantAreaName',

		[Location].[Id] AS 'LocationId',
		[Location].[Name] AS 'LocationName'
	FROM
		[User]
	LEFT JOIN [ConsultantArea]
	ON [ConsultantArea].[Id] = [User].[ConsultantAreaId]
	LEFT JOIN [Location]
	ON [Location].[Id] = [User].[LocationId]
	WHERE
		[User].[Email] = @email
GO

-- Get user by access token
CREATE PROCEDURE [GetUserByAccessToken]
	@token varchar(8000)
AS
	SELECT
		[User].[Id] AS 'UserId',
		[FirstName],
		[LastName],
		[Email],
		[AccessToken],

		[ConsultantArea].[Id] AS 'ConsultantAreaId',
		[ConsultantArea].[Name] AS 'ConsultantAreaName',

		[Location].[Id] AS 'LocationId',
		[Location].[Name] AS 'LocationName'
	FROM
		[User]
	LEFT JOIN [ConsultantArea]
	ON [ConsultantArea].[Id] = [User].[ConsultantAreaId]
	LEFT JOIN [Location]
	ON [Location].[Id] = [User].[LocationId]
	WHERE
		[User].[AccessToken] = @token
GO

-- Update User password
CREATE PROCEDURE [UpdateUserPassword]
	@email varchar(255),
	@secret varchar(255)
AS
	IF EXISTS (SELECT [Email] FROM [User] WHERE [User].[Email] = @email) 
		UPDATE [User]
		SET
			[Password] = @secret
		WHERE
			[User].[Email] = @email
	ELSE
		RETURN 0
GO