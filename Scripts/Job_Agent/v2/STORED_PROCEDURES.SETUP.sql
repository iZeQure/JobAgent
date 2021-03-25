USE [JobAgentDB_v2]
GO

DROP PROCEDURE IF EXISTS [JA.spCreateArea]
DROP PROCEDURE IF EXISTS [JA.spUpdateArea]
DROP PROCEDURE IF EXISTS [JA.spRemoveArea]
DROP PROCEDURE IF EXISTS [JA.spGetAreaById]
DROP PROCEDURE IF EXISTS [JA.spGetAreas]
GO

CREATE PROCEDURE [JA.spCreateArea] (
	@areaName varchar(50))
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'AreaInsert';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Inserting an Area';

	BEGIN TRY
		IF EXISTS (SELECT [Name] FROM [Area] WHERE [Name] = @areaName)
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Creating area failed, already exists', 4, 2
		ELSE
			INSERT INTO [Area] ([Name])
			VALUES
			(@areaName);

			COMMIT TRANSACTION @TranName;			
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error while creating area', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

CREATE PROCEDURE [JA.spUpdateArea] (
	@areaId int,
	@areaName varchar(50))
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'AreaUpdate';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Updating an Area';

	BEGIN TRY
		IF EXISTS (SELECT [Name] FROM [Area] WHERE [Name] = @areaName AND [Id] != @areaId)
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Updating area failed, already exists', 4, 2
		ELSE
			UPDATE [Area]
				SET [Name] = @areaName
			WHERE [Area].[Id] = @areaId

			COMMIT TRANSACTION @TranName
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error while updating area', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

CREATE PROCEDURE [JA.spRemoveArea] (
	@areaId int)
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'AreaRemove';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Removing an Area';

	BEGIN TRY
		IF NOT EXISTS (SELECT * FROM [Area] WHERE [Id] = @areaId)
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Removing area failed, does not exist', 4, 2
		ElSE
			-- Remove all references to the primary key.
			DELETE FROM [ConsultantArea]
			WHERE [AreaId] = @areaId

			DELETE FROM [Area]
			WHERE [Id] = @areaId

			COMMIT TRANSACTION @TranName
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error while removing area', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

CREATE PROCEDURE [JA.spGetAreaById] (
	@areaId int)
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'AreaGetById';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Getting an area by id';

	BEGIN TRY
		IF NOT EXISTS (SELECT [Id] FROM [Area] WHERE [Id] = @areaId)
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Getting area failed, does not exist', 4, 2
		ELSE
			SELECT
				[dbo].[Area].[Id] AS 'Area ID',
				[dbo].[Area].[Name] as 'Area Name'
			FROM [Area]
			WHERE [Id] = @areaId

			COMMIT TRANSACTION @TranName
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error getting area by id', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

CREATE PROCEDURE [JA.spGetAreas]
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'AreaGetAll';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Get collection of area';

	BEGIN TRY
		IF NOT EXISTS (SELECT TOP(1) [Id] FROM [Area])
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Failed getting area collection, was empty', 4, 2
		ELSE
			SELECT
				[dbo].[Area].[Id] AS 'Area ID',
				[dbo].[Area].[Name] as 'Area Name'
			FROM [Area]

			COMMIT TRANSACTION @TranName
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error getting area collection', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

DROP PROCEDURE IF EXISTS [JA.spCreateRole]
DROP PROCEDURE IF EXISTS [JA.spUpdateRole]
DROP PROCEDURE IF EXISTS [JA.spRemoveRole]
DROP PROCEDURE IF EXISTS [JA.spGetRoleById]
DROP PROCEDURE IF EXISTS [JA.spGetRoles]
GO

CREATE PROCEDURE [JA.spCreateRole] (
	@roleName varchar(30),
	@roleDesription varchar(100))
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'RoleInsert';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Inserting a Role';

	BEGIN TRY
		IF EXISTS (SELECT [Name] FROM [Role] WHERE [Name] = @roleName)
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Creating role failed, already exists', 4, 2
		ELSE
			INSERT INTO [Role] ([Name], [Description])
			VALUES
			(@roleName, @roleDesription);

			COMMIT TRANSACTION @TranName;			
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error while creating role', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

CREATE PROCEDURE [JA.spUpdateRole] (
	@roleId int,
	@roleName varchar(30),
	@roleDescription varchar(100))
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'RoleUpdate';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Updating a Role';

	BEGIN TRY
		IF EXISTS (SELECT [Name] FROM [Role] WHERE [Name] = @roleName AND [Id] != @roleId)
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Updating role failed, already exists', 4, 2
		ELSE
			UPDATE [Role]
				SET [Name] = @roleName,
					[Description] = @roleDescription
			WHERE [Role].[Id] = @roleId

			COMMIT TRANSACTION @TranName
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error while updating role', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

CREATE PROCEDURE [JA.spRemoveRole] (
	@roleId int)
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'RoleRemove';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Removing a Role';

	BEGIN TRY
		IF NOT EXISTS (SELECT * FROM [Role] WHERE [Id] = @roleId)
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Removing role failed, does not exist', 4, 2
		ElSE
			IF EXISTS (SELECT [RoleId] FROM [dbo].[User] WHERE [RoleId] = @roleId)
				EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Removing role failed, is in use', 4, 2
			ELSE
				DELETE FROM [Role]
				WHERE [Role].[Id] = @roleId

				COMMIT TRANSACTION @TranName
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error while removing role', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

CREATE PROCEDURE [JA.spGetRoleById] (
	@roleId int)
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'RoleGetById';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Getting a role by id';

	BEGIN TRY
		IF NOT EXISTS (SELECT [Id] FROM [Role] WHERE [Id] = @roleId)
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Getting role failed, does not exist', 4, 2
		ELSE
			SELECT
				[dbo].[Role].[Id] AS 'Role ID',
				[dbo].[Role].[Name] AS 'Role Name',
				[dbo].[Role].[Description] AS 'Role Description'
			FROM [Role]
			WHERE [Id] = @roleId

			COMMIT TRANSACTION @TranName
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error getting role by id', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

CREATE PROCEDURE [JA.spGetRoles]
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'RoleGetAll';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Get collection of Role';

	BEGIN TRY
		IF NOT EXISTS (SELECT TOP(1) [Id] FROM [Role])
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Failed getting role collection, was empty', 4, 2
		ELSE
			SELECT
				[dbo].[Role].[Id] AS 'Role ID',
				[dbo].[Role].[Name] AS 'Role Name',
				[dbo].[Role].[Description] AS 'Role Description'
			FROM [Role]

			COMMIT TRANSACTION @TranName
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error getting role collection', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

DROP PROCEDURE IF EXISTS [JA.spCreateLocation]
DROP PROCEDURE IF EXISTS [JA.spUpdateLocation]
DROP PROCEDURE IF EXISTS [JA.spRemoveLocation]
DROP PROCEDURE IF EXISTS [JA.spGetLocationById]
DROP PROCEDURE IF EXISTS [JA.spGetLocations]
GO

CREATE PROCEDURE [JA.spCreateLocation] (
	@locationName varchar(30))
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'LocationInsert';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Inserting a Location';

	BEGIN TRY
		IF EXISTS (SELECT [Name] FROM [Location] WHERE [Name] = @locationName)
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Creating location failed, already exists', 4, 2
		ELSE
			INSERT INTO [Location] ([Name])
			VALUES
			(@locationName);

			COMMIT TRANSACTION @TranName;			
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error while creating location', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

CREATE PROCEDURE [JA.spUpdateLocation] (
	@locationId int,
	@locationName varchar(30))
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'LocationUpdate';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Updating a Location';

	BEGIN TRY
		IF EXISTS (SELECT [Name] FROM [Location] WHERE [Name] = @locationName AND [Id] != @locationId)
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Updating location failed, already exists', 4, 2
		ELSE
			UPDATE [Location]
				SET [Name] = @locationName
			WHERE [dbo].[Location].[Id] = @locationId

			COMMIT TRANSACTION @TranName
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error while updating location', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

CREATE PROCEDURE [JA.spRemoveLocation] (
	@locationId int)
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'LocationRemove';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Removing a Location';

	BEGIN TRY
		IF NOT EXISTS (SELECT * FROM [Location] WHERE [Id] = @locationId)
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Removing location failed, does not exist', 4, 2
		ElSE
			IF EXISTS (SELECT [LocationId] FROM [dbo].[User] WHERE [LocationId] = @locationId)
				EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Removing location failed, is in use', 4, 2
			ELSE
				DELETE FROM [Location]
				WHERE [Location].[Id] = @locationId

				COMMIT TRANSACTION @TranName
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error while removing location', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

CREATE PROCEDURE [JA.spGetLocationById] (
	@locationId int)
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'LocationGetById';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Getting a location by id';

	BEGIN TRY
		IF NOT EXISTS (SELECT [Id] FROM [Location] WHERE [Id] = @locationId)
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Getting location failed, does not exist', 4, 2
		ELSE
			SELECT
				[dbo].[Location].[Id] AS 'Location ID',
				[dbo].[Location].[Name] AS 'Location Name'
			FROM [Location]
			WHERE [Id] = @locationId

			COMMIT TRANSACTION @TranName
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error getting location by id', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

CREATE PROCEDURE [JA.spGetLocations]
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'LocationGetAll';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Get collection of Location';

	BEGIN TRY
		IF NOT EXISTS (SELECT TOP(1) [Id] FROM [Location])
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Failed getting location collection, was empty', 4, 2
		ELSE
			SELECT
				[dbo].[Location].[Id] AS 'Location ID',
				[dbo].[Location].[Name] AS 'Location Name'
			FROM [Location]

			COMMIT TRANSACTION @TranName
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error getting location collection', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

DROP PROCEDURE IF EXISTS [JA.spCreateContract]
DROP PROCEDURE IF EXISTS [JA.spUpdateContract]
DROP PROCEDURE IF EXISTS [JA.spRemoveContract]
DROP PROCEDURE IF EXISTS [JA.spGetContractById]
DROP PROCEDURE IF EXISTS [JA.spGetContracts]
GO

CREATE PROCEDURE [JA.spCreateContract] (
	@companyCVR int,
	@userId int,
	@name uniqueidentifier)
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'ContractInsert';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Inserting a Contract';

	BEGIN TRY
		IF EXISTS (SELECT [CompanyCVR] FROM [Contract] WHERE [CompanyCVR] = @companyCVR)
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Creating contract failed, already exists', 4, 2
		ELSE			
			IF NOT EXISTS (SELECT [Id] FROM [User] WHERE [Id] = 1)
			AND NOT EXISTS (SELECT [CVR] FROM [Company] WHERE [CVR] = 1)
				EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Creating contract failed, user and company does not exist', 4, 2
			ELSE
				INSERT INTO [Contract] ([CompanyCVR], [UserId], [Name], [RegistrationDateTime], [ExpiryDateTime])
				VALUES
				(@companyCVR, @userId, @name, GETDATE(), DATEADD(YEAR, 5, GETDATE()));

				COMMIT TRANSACTION @TranName;
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error while creating contract', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

CREATE PROCEDURE [JA.spUpdateArea] (
	@areaId int,
	@areaName varchar(50))
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'AreaUpdate';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Updating an Area';

	BEGIN TRY
		IF EXISTS (SELECT [Name] FROM [Area] WHERE [Name] = @areaName AND [Id] != @areaId)
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Updating area failed, already exists', 4, 2
		ELSE
			UPDATE [Area]
				SET [Name] = @areaName
			WHERE [Area].[Id] = @areaId

			COMMIT TRANSACTION @TranName
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error while updating area', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

CREATE PROCEDURE [JA.spRemoveArea] (
	@areaId int)
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'AreaRemove';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Removing an Area';

	BEGIN TRY
		IF NOT EXISTS (SELECT * FROM [Area] WHERE [Id] = @areaId)
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Removing area failed, does not exist', 4, 2
		ElSE
			-- Remove all references to the primary key.
			DELETE FROM [ConsultantArea]
			WHERE [AreaId] = @areaId

			DELETE FROM [Area]
			WHERE [Id] = @areaId

			COMMIT TRANSACTION @TranName
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error while removing area', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

CREATE PROCEDURE [JA.spGetAreaById] (
	@areaId int)
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'AreaGetById';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Getting an area by id';

	BEGIN TRY
		IF NOT EXISTS (SELECT [Id] FROM [Area] WHERE [Id] = @areaId)
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Getting area failed, does not exist', 4, 2
		ELSE
			SELECT
				[dbo].[Area].[Id] AS 'Area ID',
				[dbo].[Area].[Name] as 'Area Name'
			FROM [Area]
			WHERE [Id] = @areaId

			COMMIT TRANSACTION @TranName
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error getting area by id', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

CREATE PROCEDURE [JA.spGetAreas]
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'AreaGetAll';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Get collection of area';

	BEGIN TRY
		IF NOT EXISTS (SELECT TOP(1) [Id] FROM [Area])
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Failed getting area collection, was empty', 4, 2
		ELSE
			SELECT
				[dbo].[Area].[Id] AS 'Area ID',
				[dbo].[Area].[Name] as 'Area Name'
			FROM [Area]

			COMMIT TRANSACTION @TranName
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error getting area collection', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

DROP PROCEDURE IF EXISTS [JA.spCreateCompany]
DROP PROCEDURE IF EXISTS [JA.spUpdateCompany]
DROP PROCEDURE IF EXISTS [JA.spRemoveCompany]
DROP PROCEDURE IF EXISTS [JA.spGetCompanyById]
DROP PROCEDURE IF EXISTS [JA.spGetCompanies]

DROP PROCEDURE IF EXISTS [JA.spCreateVacantJob]
DROP PROCEDURE IF EXISTS [JA.spUpdateVacantJob]
DROP PROCEDURE IF EXISTS [JA.spRemoveVacantJob]
DROP PROCEDURE IF EXISTS [JA.spGetVacantJobId]
DROP PROCEDURE IF EXISTS [JA.spGetVacantJobs]

DROP PROCEDURE IF EXISTS [JA.spCreateCategory]
DROP PROCEDURE IF EXISTS [JA.spUpdateCategory]
DROP PROCEDURE IF EXISTS [JA.spRemoveCategory]
DROP PROCEDURE IF EXISTS [JA.spGetCategoryById]
DROP PROCEDURE IF EXISTS [JA.spGetCategories]
DROP PROCEDURE IF EXISTS [JA.spGetCategoryMenu] -- Returns Catory with associated Specializations

DROP PROCEDURE IF EXISTS [JA.spCreateSpecialization]
DROP PROCEDURE IF EXISTS [JA.spUpdateSpecialization]
DROP PROCEDURE IF EXISTS [JA.spRemoveSpecialization]
DROP PROCEDURE IF EXISTS [JA.spGetSpecializationById]
DROP PROCEDURE IF EXISTS [JA.spGetSpecializations]

DROP PROCEDURE IF EXISTS [JA.spCreateJobAdvert]
DROP PROCEDURE IF EXISTS [JA.spUpdateJobAdvert]
DROP PROCEDURE IF EXISTS [JA.spRemoveJobAdvert]
DROP PROCEDURE IF EXISTS [JA.spGetJobAdvertById]
DROP PROCEDURE IF EXISTS [JA.spGetJobAdverts]
DROP PROCEDURE IF EXISTS [JA.spGetTotalJobAdvertCountByCategoryId] -- Returns Integer
DROP PROCEDURE IF EXISTS [JA.spGetTotalJobAdvertCountBySpecializationId] -- Returns Integer
DROP PROCEDURE IF EXISTS [JA.spGetTotalJobAdvertCountByNonCategorized] -- Returns Integer

DROP PROCEDURE IF EXISTS [JA.spCreateUser]
DROP PROCEDURE IF EXISTS [JA.spUpdateUser]
DROP PROCEDURE IF EXISTS [JA.spRemoveUser]
DROP PROCEDURE IF EXISTS [JA.spGetUserById]
DROP PROCEDURE IF EXISTS [JA.spGetUsers]
DROP PROCEDURE IF EXISTS [JA.spGrantUserArea]
DROP PROCEDURE IF EXISTS [JA.spRemoveUserArea]
DROP PROCEDURE IF EXISTS [JA.spGetUserSaltByEmail]
DROP PROCEDURE IF EXISTS [JA.spGetUserByAccessToken]
DROP PROCEDURE IF EXISTS [JA.spUpdateUserSecurity]
DROP PROCEDURE IF EXISTS [JA.spValidateUserExists] -- Returns Bit
DROP PROCEDURE IF EXISTS [JA.spValidateUserLogin] -- Returns Bit
GO