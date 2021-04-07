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
		IF NOT EXISTS (SELECT * FROM [Area] WHERE [Id] != @areaId)
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Updating area failed, does not exist', 4, 2
		ELSE
			IF EXISTS (SELECT * FROM [Area] WHERE [Name] = @areaName)
				EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Updating area failed, area already exists', 4, 2
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
		IF NOT EXISTS (SELECT * FROM [Role] WHERE [Id] != @roleId)
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Updating role failed, does not exist', 4, 2
		ELSE
			IF EXISTS (SELECT * FROM [Role] WHERE [Name] = @roleName)
				EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Updating role failed, role already exists', 4, 2
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
		IF NOT EXISTS (SELECT [Name] FROM [Location] WHERE [Id] != @locationId)
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Updating location failed, does not exist', 4, 2
		ELSE
			IF EXISTS (SELECT * FROM [Location] WHERE [Name] = @locationName)
				EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Updating location failed, location already exists', 4, 2
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
	@companyId int,
	@userId int,
	@name uniqueidentifier)
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'ContractInsert';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Inserting a Contract';

	BEGIN TRY
		IF EXISTS (SELECT * FROM [Contract] WHERE [CompanyId] = @companyId)
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Creating contract failed, already exists', 4, 2
		ELSE			
			IF NOT EXISTS (SELECT [Id] FROM [User] WHERE [Id] = @userId)
			AND NOT EXISTS (SELECT [Id] FROM [Company] WHERE [Id] = @companyId)
				EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Creating contract failed, user and company does not exist', 4, 2
			ELSE
				INSERT INTO [Contract] ([CompanyId], [UserId], [Name], [RegistrationDateTime], [ExpiryDateTime])
				VALUES
				(@companyId, @userId, @name, GETDATE(), DATEADD(YEAR, 5, GETDATE()));

				COMMIT TRANSACTION @TranName;
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error while creating contract', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

CREATE PROCEDURE [JA.spUpdateContract] (
	@contractId int,
	@companyId int,
	@userId varchar(50),
	@name uniqueidentifier,
	@registrationDateTime datetime,
	@expiryDateTime datetime)
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'ContractUpdate';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Updating a Contract';

	BEGIN TRY
		IF NOT EXISTS (SELECT * FROM [Contract] WHERE [Id] = @contractId)
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Updating contract failed, does not exist', 4, 2
		ELSE
			UPDATE [Contract]
				SET 
					[CompanyId] = @companyId,
					[UserId] = @userId,
					[Name] = @name,
					[RegistrationDateTime] = @registrationDateTime,
					[ExpiryDateTime] = @expiryDateTime
			WHERE [Contract].[Id] = @contractId

			COMMIT TRANSACTION @TranName
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error while updating contract', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

CREATE PROCEDURE [JA.spRemoveContract] (
	@contractId int)
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'ContractRemove';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Removing a Contract';

	BEGIN TRY
		IF NOT EXISTS (SELECT * FROM [Contract] WHERE [Id] = @contractId)
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Removing contract failed, does not exist', 4, 2
		ElSE
			DELETE FROM [Contract]
			WHERE [Id] = @contractId

			COMMIT TRANSACTION @TranName
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error while removing contract', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

CREATE PROCEDURE [JA.spGetContractById] (
	@contractId int)
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'ContractGetById';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Getting a contract by id';

	BEGIN TRY
		IF NOT EXISTS (SELECT [Id] FROM [Contract] WHERE [Id] = @contractId)
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Getting contract failed, does not exist', 4, 2
		ELSE
			SELECT
				[dbo].[Contract].[Id] AS 'Contract ID',
				[dbo].[Contract].[CompanyId] AS 'Company ID',
				[dbo].[Contract].[UserId] AS 'User ID',
				[dbo].[Contract].[Name] AS 'Contract Name',
				[dbo].[Contract].[RegistrationDateTime] AS 'Registered Date',
				[dbo].[Contract].[ExpiryDateTime] AS 'Expiration Date'
			FROM [Contract]
			WHERE [Id] = @contractid

			COMMIT TRANSACTION @TranName
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error getting contract by id', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

CREATE PROCEDURE [JA.spGetContracts]
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'ContractGetAll';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Get collection of contract';

	BEGIN TRY
		IF NOT EXISTS (SELECT TOP(1) [Id] FROM [Contract])
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Failed getting contract collection, was empty', 4, 2
		ELSE
			SELECT
				[dbo].[Contract].[Id] AS 'Contract ID',
				[dbo].[Contract].[CompanyId] AS 'Contract ID',
				[dbo].[Contract].[UserId] AS 'User ID',
				[dbo].[Contract].[Name] AS 'Contract Name',
				[dbo].[Contract].[RegistrationDateTime] AS 'Registered Date',
				[dbo].[Contract].[ExpiryDateTime] AS 'Expiration Date'
			FROM [Contract]

			COMMIT TRANSACTION @TranName
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error getting contract collection', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

DROP PROCEDURE IF EXISTS [JA.spCreateCompany]
DROP PROCEDURE IF EXISTS [JA.spUpdateCompany]
DROP PROCEDURE IF EXISTS [JA.spRemoveCompany]
DROP PROCEDURE IF EXISTS [JA.spGetCompanyById]
DROP PROCEDURE IF EXISTS [JA.spGetCompanies]
GO

CREATE PROCEDURE [JA.spCreateCompany] (
	@companyCVR int,
	@companyName varchar(50),
	@contactPerson varchar(50),
	@jobPageUrl varchar(2048))
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'CompanyInsert';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Inserting a Company';

	BEGIN TRY
		IF EXISTS (SELECT * FROM [Company] WHERE [CVR] = @companyCVR)
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Creating company failed, already exists', 4, 2
		ELSE
			INSERT INTO [Company] ([CVR], [Name], [ContactPerson], [JobPageURL])
			VALUES
			(@CompanyCVR, @companyname, @contactPerson, @jobPageUrl);

			COMMIT TRANSACTION @TranName;			
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error while creating company', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

CREATE PROCEDURE [JA.spUpdateCompany] (
    @companyId int,
	@companyCVR int,
	@companyName varchar(50),
	@contactPerson varchar(50),
	@jobPageUrl varchar(2048))
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'CompanyUpdate';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Updating a Company';

	BEGIN TRY
		IF NOT EXISTS (SELECT [Id] FROM [Company] WHERE [Id] = @companyId)
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Updating Company failed, already exists', 4, 2
		ELSE
			UPDATE [Company]
				SET [CVR] = @companyCVR,
				    [Name] = @companyName,
					[ContactPerson] = @contactPerson,
					[JobPageURL] = @jobPageUrl
			WHERE [Company].[CVR] = @companyCVR

			COMMIT TRANSACTION @TranName
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error while updating Company', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

CREATE PROCEDURE [JA.spRemoveCompany] (
	@companyId int)
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'CompanyRemove';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Removing an Company';

	BEGIN TRY
		IF NOT EXISTS (SELECT * FROM [Company] WHERE [Id] = @companyId)
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Removing Company failed, does not exist', 4, 2
		ElSE
			DECLARE @vacantJobId int
			SET @vacantJobId = (SELECT [Id] FROM [VacantJob] WHERE [CompanyId] = @companyId)

			-- Remove all references to the primary key.
			DELETE FROM [JobAdvert] WHERE [VacantJobId] = @vacantJobId;
			DELETE FROM [VacantJob] WHERE [CompanyId] = @companyId;
			DELETE FROM [Contract] WHERE [CompanyId] = @companyId;
			DELETE FROM [Company] WHERE [Id] = @companyId;

			COMMIT TRANSACTION @TranName
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error while removing company', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

CREATE PROCEDURE [JA.spGetCompanyById] (
	@companyId int)
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'CompanyGetById';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Getting a Company by ID';

	BEGIN TRY
		IF NOT EXISTS (SELECT [Id] FROM [Company] WHERE [Id] = @companyId)
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Getting Company failed, does not exist', 4, 2
		ELSE
			SELECT
			    [dbo].[Company].[CVR] AS 'Company CVR',
				[dbo].[Company].[Name] AS 'Company Name',
				[dbo].[Company].[ContactPerson] AS 'Contact Person',
				[dbo].[Company].[JobPageURL] AS 'Jobpage Url'
			FROM [Company]
			WHERE [Id] = @companyId

			COMMIT TRANSACTION @TranName
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error getting Company by ID', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

CREATE PROCEDURE [JA.spGetCompanies]
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'CompanyGetAll';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Get collection of Company';

	BEGIN TRY
		IF NOT EXISTS (SELECT TOP(1) [Id] FROM [Company])
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Failed getting company collection, was empty', 4, 2
		ELSE
			SELECT
				[dbo].[Company].[CVR] AS 'Company CVR',
				[dbo].[Company].[Name] as 'Company Name',
				[dbo].[Company].[ContactPerson] as 'Contact person',
				[dbo].[Company].JobPageURL as 'JobPage Url'
			FROM [Company]

			COMMIT TRANSACTION @TranName
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error getting Company collection', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

DROP PROCEDURE IF EXISTS [JA.spCreateVacantJob]
DROP PROCEDURE IF EXISTS [JA.spUpdateVacantJob]
DROP PROCEDURE IF EXISTS [JA.spRemoveVacantJob]
DROP PROCEDURE IF EXISTS [JA.spGetVacantJobById]
DROP PROCEDURE IF EXISTS [JA.spGetVacantJobs]
GO

CREATE PROCEDURE [JA.spCreateVacantJob] (
	@vacantJobLink varchar(max),
	@companyId int)
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'VacantJobInsert';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Inserting a VacantJob';

	BEGIN TRY
		IF EXISTS (SELECT * FROM [VacantJob] WHERE [Link] = @vacantJobLink)
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Creating VacantJob failed, already exists', 4, 2
		ELSE
			IF NOT EXISTS (SELECT * FROM [Company] WHERE [Id] = @companyId)
				EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Creating VacantJob failed, company does not exist', 4, 2	
			ELSE
				INSERT INTO [VacantJob] ([Link], [CompanyId])
				VALUES
				(@vacantJobLink, @companyId);

				COMMIT TRANSACTION @TranName;			
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error while creating VacantJob', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

CREATE PROCEDURE [JA.spUpdateVacantJob] (
	@vacantJobId int,
	@vacantJobLink varchar(max),
	@companyId int)
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'VacantJobUpdate';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Updating a VacantJob';

	BEGIN TRY
		IF NOT EXISTS (SELECT [Id] FROM [VacantJob] WHERE [Id] = @vacantJobId)
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Updating VacantJob failed, does not exist', 4, 2
		ELSE
			IF EXISTS (SELECT * FROM [VacantJob] WHERE [Link] = @vacantJobLink)
				EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Updating VacantJob failed, link already exists', 4, 2
			ELSE
				IF NOT EXISTS (SELECT * FROM [Company] WHERE [Id] = @companyId)
					EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Updating VacantJob failed, company does not exist', 4, 2
				ELSE
					UPDATE [VacantJob]
						SET [Link] = @vacantJobLink,
							[CompanyId] = @companyId
					WHERE [VacantJob].[Id] = @vacantJobId

					COMMIT TRANSACTION @TranName
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error while updating VacantJob', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

CREATE PROCEDURE [JA.spRemoveVacantJob] (
	@vacantJobId int)
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'VacantJobRemove';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Removing a VacantJob';

	BEGIN TRY
		IF NOT EXISTS (SELECT * FROM [VacantJob] WHERE [Id] = @vacantJobId)
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Removing VacantJob failed, does not exist', 4, 2
		ElSE
			-- Remove all references to the primary key.
			DELETE FROM [JobAdvert] WHERE [VacantJobId] = @vacantJobId;
			DELETE FROM [VacantJob] WHERE [Id] = @vacantJobId;

			COMMIT TRANSACTION @TranName
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error while removing VacantJob', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

CREATE PROCEDURE [JA.spGetVacantJobById] (
	@vacantJobId int)
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'VacantJobById';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Getting a vacantjob by Id';

	BEGIN TRY
		IF NOT EXISTS (SELECT [Id] FROM [VacantJob] WHERE [Id] = @vacantJobId)
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Getting Vacantjob failed, does not exist', 4, 2
		ELSE
			SELECT
			    [dbo].[VacantJob].[Id] AS 'Vacant job ID',
				[dbo].[VacantJob].[Link] AS 'VacantJob Link',
				[dbo].[VacantJob].[CompanyId] AS 'Company ID'
				
			FROM [VacantJob]
			WHERE [Id] = @vacantJobId

			COMMIT TRANSACTION @TranName
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error getting VacantJob by ID', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

CREATE PROCEDURE [JA.spGetVacantJobs]
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'VacantJobGetAll';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Get collection of VacantJob';

	BEGIN TRY
		IF NOT EXISTS (SELECT TOP(1) [Id] FROM [VacantJob])
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Failed getting Vacantjob collection, was empty', 4, 2
		ELSE
			SELECT
				[dbo].[VacantJob].[Id] AS 'Vacant job ID',
				[dbo].[VacantJob].[Link] AS 'VacantJob Link',
				[dbo].[VacantJob].[CompanyId] AS 'Company ID'
			FROM [VacantJob]

			COMMIT TRANSACTION @TranName
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error getting VacantJob collection', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

DROP PROCEDURE IF EXISTS [JA.spCreateCategory]
DROP PROCEDURE IF EXISTS [JA.spUpdateCategory]
DROP PROCEDURE IF EXISTS [JA.spRemoveCategory]
DROP PROCEDURE IF EXISTS [JA.spGetCategoryById]
DROP PROCEDURE IF EXISTS [JA.spGetCategories]
DROP PROCEDURE IF EXISTS [JA.spGetCategoryMenu] -- Returns Catory with associated Specializations
GO

CREATE PROCEDURE [JA.spCreateCategory] (
	@categoryName varchar(100))
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'CategoryInsert';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Inserting a category';

	BEGIN TRY
		IF EXISTS (SELECT * FROM [Category] WHERE [Name] = @categoryName)
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Creating category failed, already exists', 4, 2
		ELSE
			INSERT INTO [Category] ([Name])
			VALUES
			(@categoryName);

			COMMIT TRANSACTION @TranName;			
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error while creating category', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

CREATE PROCEDURE [JA.spUpdateCategory] (
	@categoryId int,
	@categoryName varchar(100))
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'CategoryUpdate';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Updating a category';

	BEGIN TRY
		IF NOT EXISTS (SELECT [Id] FROM [Category] WHERE [Id] = @categoryId)
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Updating category failed, does not exist', 4, 2
		ELSE
			IF EXISTS (SELECT * FROM [Category] WHERE [Name] = @categoryName)
				EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Updating category failed, name already exists', 4, 2
			ElSE
				UPDATE [Category]
					SET [Name] = @categoryName
				WHERE [Category].[Id] = @categoryId

				COMMIT TRANSACTION @TranName
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error while updating category', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

CREATE PROCEDURE [JA.spRemoveCategory] (
	@categoryId int)
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'categoryRemove';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Removing a Category';

	BEGIN TRY
		IF NOT EXISTS (SELECT * FROM [Category] WHERE [Id] = @categoryId)
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Removing category failed, does not exist', 4, 2
		ElSE
			-- Remove all references to the primary key.
			DELETE FROM [JobAdvert] WHERE [CategoryId] = @categoryId;
			DELETE FROM [Category]	WHERE [Id] = @categoryId;

			COMMIT TRANSACTION @TranName
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error while removing category', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

CREATE PROCEDURE [JA.spGetCategoryById] (
	@categoryId int,
	@categoryName varchar(100))
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'CategoryById';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Getting a Category by Id';

	BEGIN TRY
		IF NOT EXISTS (SELECT [Id] FROM [Category] WHERE [Id] = @categoryId)
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Getting category failed, does not exist', 4, 2
		ELSE
			SELECT
			    [dbo].[Category].[Id] AS 'Category ID',
				[dbo].[Category].[Name] AS 'Category Name'
				
				
			FROM [Category]
			WHERE [Id] = @categoryId

			COMMIT TRANSACTION @TranName
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error getting category by ID', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

CREATE PROCEDURE [JA.spGetCategories]
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'CategoryGetAll';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Get collection of Category';

	BEGIN TRY
		IF NOT EXISTS (SELECT TOP(1) [Id] FROM [VacantJob])
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Failed getting Category collection, was empty', 4, 2
		ELSE
			SELECT
			    [dbo].[Category].[Id] AS 'Category ID',
				[dbo].[Category].[Name] AS 'Category Name'
				
			FROM [Category]

			COMMIT TRANSACTION @TranName
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error getting Category collection', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

CREATE PROCEDURE [JA.spGetCategoryMenu]
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'CategoryGetMenu';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Get Category Menu';

	BEGIN TRY
		SELECT
			[Category].[Id] AS 'Category ID',
			[Category].[Name] AS 'Category Name',

			[Specialization].[Id] AS 'Spec Id',
			[Specialization].[Name] AS 'Specialization Name',
			[Specialization].[CategoryId] AS 'Specialization Category ID'
		FROM [Category]
		LEFT OUTER JOIN [Specialization] ON [Category].[Id] = [Specialization].[CategoryId]

		COMMIT TRANSACTION @TranName
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error getting Category Menu', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

DROP PROCEDURE IF EXISTS [JA.spCreateSpecialization]
DROP PROCEDURE IF EXISTS [JA.spUpdateSpecialization]
DROP PROCEDURE IF EXISTS [JA.spRemoveSpecialization]
DROP PROCEDURE IF EXISTS [JA.spGetSpecializationById]
DROP PROCEDURE IF EXISTS [JA.spGetSpecializations]
GO

CREATE PROCEDURE [JA.spCreateSpecialization] (
	@specializationName varchar(100),
	@categoryId int)
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'SpecializationInsert';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Inserting a Specialization';

	BEGIN TRY
		IF EXISTS (SELECT * FROM [Specialization] WHERE [Name] = @specializationName)
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Creating Specialization failed, already exists', 4, 2
		ELSE
			INSERT INTO [Specialization] ([Name], [CategoryId])
			VALUES
			(@specializationName, @categoryId);

			COMMIT TRANSACTION @TranName;			
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error while creating Specialization', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

CREATE PROCEDURE [JA.spUpdateSpecialization] (
	@specializationId int,
	@specializationName varchar(100),
	@categoryId int)
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'SpecializationUpdate';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Updating a Specialization';

	BEGIN TRY
		IF NOT EXISTS (SELECT * FROM [Specialization] WHERE [Id] = @specializationId)
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Updating Specialization failed, does not exist', 4, 2
		ELSE
			IF EXISTS (SELECT * FROM [Specialization] WHERE [Name] = @specializationName AND [Id] != @specializationId)
				EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Updating Specialization failed, duplicate of row', 4, 2	
			ELSE
				UPDATE [Specialization]
					SET [Name] = @specializationName,
						[CategoryId] = @categoryId
					WHERE [Id] = @specializationId

				COMMIT TRANSACTION @TranName
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error while updating Specialization', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

CREATE PROCEDURE [JA.spRemoveSpecialization] (
	@specializationId int)
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'SpecializationRemove';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Removing a Specialization';

	BEGIN TRY
		IF NOT EXISTS (SELECT * FROM [Specialization] WHERE [Id] = @specializationId)
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Removing Specialization failed, does not exist', 4, 2
		ElSE			
			DELETE FROM [JobAdvert] WHERE [SpecializationId] = @specializationId;
			DELETE FROM [Specialization] WHERE [Id] = @specializationId;

			COMMIT TRANSACTION @TranName;
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error while removing Specialization', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

CREATE PROCEDURE [JA.spGetSpecializationById] (
	@specializationId int)
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'SpecializationById';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Getting a Specialization by Id';

	BEGIN TRY
		IF NOT EXISTS (SELECT [Id] FROM [Specialization] WHERE [Id] = @specializationId)
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Getting Specialization failed, does not exist', 4, 2
		ELSE
			SELECT
				[dbo].[Specialization].[Id] AS 'Specialization ID',
				[dbo].[Specialization].[Name] AS 'Specialization Name',
				[dbo].[Specialization].[CategoryId] AS 'Category ID'
			FROM
				[Specialization] WHERE [Id] = @specializationId

			COMMIT TRANSACTION @TranName
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error getting Specialization by ID', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

CREATE PROCEDURE [JA.spGetSpecializations]
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'SpecializationGetAll';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Get collection of Specialization';

	BEGIN TRY
		IF NOT EXISTS (SELECT TOP(1) [Id] FROM [Specialization])
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Failed getting Specialization collection, was empty', 4, 2
		ELSE
			SELECT
				[dbo].[Specialization].[Id] AS 'Specialization ID',
				[dbo].[Specialization].[Name] AS 'Specialization Name',
				[dbo].[Specialization].[CategoryId] AS 'Category ID'
			FROM
				[Specialization]

			COMMIT TRANSACTION @TranName
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error getting Specialization collection', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

DROP PROCEDURE IF EXISTS [JA.spCreateJobAdvert]
DROP PROCEDURE IF EXISTS [JA.spUpdateJobAdvert]
DROP PROCEDURE IF EXISTS [JA.spRemoveJobAdvert]
DROP PROCEDURE IF EXISTS [JA.spGetJobAdvertById]
DROP PROCEDURE IF EXISTS [JA.spGetJobAdverts]
DROP PROCEDURE IF EXISTS [JA.spGetTotalJobAdvertCountByCategoryId] -- Returns Integer
DROP PROCEDURE IF EXISTS [JA.spGetTotalJobAdvertCountBySpecializationId] -- Returns Integer
DROP PROCEDURE IF EXISTS [JA.spGetTotalJobAdvertCountByNonCategorized] -- Returns Integer
GO

CREATE PROCEDURE [JA.spCreateJobAdvert] (
	@vacantJobId int,
    @specializationId int,
	@categoryId int,
	@jobAdvertTitle varchar(100),
	@jobAdvertSummary varchar(250),
	@jobAdvertDescription varchar(max),
	@jobAdvertEmail varchar(50),
	@jobAdvertPhoneNr varchar(25),
	@jobAdvertRegistrationDateTime datetime,
	@jobAdvertApplicationDeadlineDateTime datetime)
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'JobAdvertInsert';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Inserting a JobAdvert';

	BEGIN TRY
		IF EXISTS (SELECT * FROM [JobAdvert] WHERE [VacantJobId] = @vacantJobId)
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Creating JobAdvert failed, already exists', 4, 2
		 
		ELSE
			IF NOT EXISTS (SELECT * FROM [Category] WHERE [Id] = @categoryId) OR NOT EXISTS (SELECT * From [Specialization] WHERE [Id] = @specializationId)
				EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Creating JobAdvert failed, Category or specilization does not exist', 4, 2

			ELSE
			    INSERT INTO [JobAdvert] ([VacantJobId], [CategoryId], [SpecializationId], [Title], [Summary], [Description], [Email], [PhoneNumber], [RegistrationDateTime], [ApplicationDeadlineDateTime])
				VALUES
				(@vacantJobId, @specializationId, @categoryId, @jobAdvertTitle, @jobAdvertSummary, @jobAdvertDescription, @jobAdvertEmail, @jobAdvertPhoneNr, @jobAdvertRegistrationDateTime, @jobAdvertApplicationDeadlineDateTime);

				COMMIT TRANSACTION @TranName;			
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error while creating JobAdvert', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

CREATE PROCEDURE [JA.spUpdateJobAdvert] (
	@vacantJobId int,
    @specializationId int,
	@categoryId int,
	@jobAdvertTitle varchar(100),
	@jobAdvertSummary varchar(250),
	@jobAdvertDescription varchar(max),
	@jobAdvertEmail varchar(50),
	@jobAdvertPhoneNr varchar(25),
	@jobAdvertRegistrationDateTime datetime,
	@jobAdvertApplicationDeadlineDateTime datetime)
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'JobAdvertUpdate';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Updating a JobAdvert';

	BEGIN TRY
		IF NOT EXISTS (SELECT * FROM [JobAdvert] WHERE [VacantJobId] = @vacantJobId)
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Updating JobAdvert failed, does not exist', 4, 2
		ELSE
			IF NOT EXISTS (SELECT * FROM [Category] WHERE [Id] = @categoryId) OR NOT EXISTS (SELECT * From [Specialization] WHERE [Id] = @specializationId)
				EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Updating JobAdvert failed, Category or specilization does not exist', 4, 2
			ELSE
				UPDATE [JobAdvert]
					SET	
					[CategoryId] = @categoryId,
					[SpecializationId] = @specializationId,
					[Title] = @jobAdvertTitle,
					[Summary] = @jobAdvertSummary,
					[Description] = @jobAdvertDescription,
					[Email] = @jobAdvertEmail,
					[PhoneNumber] = @jobAdvertPhoneNr,
					[RegistrationDateTime] = @jobAdvertRegistrationDateTime,
					[ApplicationDeadlineDateTime] = @jobAdvertApplicationDeadlineDateTime
					WHERE [VacantJobId] = @vacantJobId

				COMMIT TRANSACTION @TranName
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error while updating JobAdvert', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

CREATE PROCEDURE [JA.spRemoveJobAdvert] (
	@vacantJobId int)
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'JobAdvertRemove';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Removing a JobAdvert';

	BEGIN TRY
		IF NOT EXISTS (SELECT * FROM [JobAdvert] WHERE [VacantJobId] = @vacantJobId)
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Removing JobAdvert failed, does not exist', 4, 2
		ElSE
			DELETE FROM [Address] WHERE [JobAdvertVacantJobId] = @vacantJobId;
			DELETE FROM [JobAdvert] WHERE [VacantJobId] = @vacantJobId;

			COMMIT TRANSACTION @TranName;
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error while removing JobAdvert', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

CREATE PROCEDURE [JA.spGetJobAdvertById] (
	@vacantJobId int)
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'JobAdvertById';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Getting a JobAdvert by Id';

	BEGIN TRY
		IF NOT EXISTS (SELECT * FROM [JobAdvert] WHERE [VacantJobId] = @vacantJobId)
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Getting JobAdvert failed, does not exist', 4, 2
		ELSE
			SELECT
				j.[VacantJobId] AS 'JobAdvert ID',
				j.[CategoryId] AS 'Category ID',
				j.[SpecializationId] AS 'Specialization ID',
				j.[Title] AS 'JobAdvert Title',
				j.[Summary] AS 'JobAdvert Summary',
				j.[Description] AS 'JobAdvert Description',
				j.[Email] AS 'JobAdvert Email',
				j.[PhoneNumber] AS 'JobAdvert Phone Number',
				j.[RegistrationDateTime] AS 'JobAdvert Registration Date',
				j.[ApplicationDeadlineDateTime] AS 'JobAdvert Application Deadline Date'
			FROM
				[JobAdvert] j WHERE [VacantJobId] = @vacantJobId

			COMMIT TRANSACTION @TranName
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error getting JobAdvert by ID', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

CREATE PROCEDURE [JA.spGetJobAdverts]
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'JobAdvertGetAll';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Get collection of JobAdvert';

	BEGIN TRY
		IF NOT EXISTS (SELECT TOP(1) * FROM [JobAdvert])
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Failed getting JobAdvert collection, was empty', 4, 2
		ELSE
			SELECT
				j.[VacantJobId] AS 'JobAdvert ID',
				j.[CategoryId] AS 'Category ID',
				j.[SpecializationId] AS 'Specialization ID',
				j.[Title] AS 'JobAdvert Title',
				j.[Summary] AS 'JobAdvert Summary',
				j.[Description] AS 'JobAdvert Description',
				j.[Email] AS 'JobAdvert Email',
				j.[PhoneNumber] AS 'JobAdvert Phone Number',
				j.[RegistrationDateTime] AS 'JobAdvert Registration Date',
				j.[ApplicationDeadlineDateTime] AS 'JobAdvert Application Deadline Date'
			FROM
				[JobAdvert] j

			COMMIT TRANSACTION @TranName
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error getting JobAdvert collection', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

CREATE PROCEDURE [JA.spGetTotalJobAdvertCountByCategoryId](
	@categoryId int,
	@countByCategory int OUTPUT)
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'JobAdvertGetCountByCategoryId';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Get count by Category';

	BEGIN TRY
		SET @countByCategory = (SELECT COUNT (*) FROM [JobAdvert] WHERE [CategoryId] = @categoryId)

		COMMIT TRANSACTION @TranName
		
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error getting count by Category', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

CREATE PROCEDURE [JA.spGetTotalJobAdvertCountBySpecializationId](
	@specializationId int,
	@countBySpecialization int OUTPUT)
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'JobAdvertGetCountBySpecializationId';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Get count by Specialization';

	BEGIN TRY
		SET @countBySpecialization = (SELECT COUNT (*) FROM [JobAdvert] WHERE [SpecializationId] = @specializationId)

		COMMIT TRANSACTION @TranName
		
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error getting count by Specialization', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

CREATE PROCEDURE [JA.spGetTotalJobAdvertCountByNonCategorized](
	@countByNonCategorized int OUTPUT)
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'JobAdvertGetCountByNonCategorized';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Get count by NonCategorized JobAdvert';

	BEGIN TRY
		SET @countByNonCategorized = (SELECT COUNT (*) FROM [JobAdvert] WHERE [CategoryId] = 0 AND [SpecializationId] = 0)

			COMMIT TRANSACTION @TranName
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error getting count by NonCategorized JobAdvert', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

DROP PROCEDURE IF EXISTS [JA.spCreateAddress]
DROP PROCEDURE IF EXISTS [JA.spUpdateAddress]
DROP PROCEDURE IF EXISTS [JA.spRemoveAddress]
DROP PROCEDURE IF EXISTS [JA.spGetAddressById]
DROP PROCEDURE IF EXISTS [JA.spGetAddresses]
GO

CREATE PROCEDURE [JA.spCreateAddress](
	@jobAdvertVacantJobId int,
	@streetAddress varchar(250),
	@city varchar(100),
	@country varchar(100),
	@postalCode varchar(10))
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'AddressInsert';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Inserting an Address';

	BEGIN TRY
		IF EXISTS (SELECT * FROM [Address] WHERE [JobAdvertVacantJobId] = @jobAdvertVacantJobId)
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Creating Address failed, already exists', 4, 2
		 ELSE
			INSERT INTO [Address] ([JobAdvertVacantJobId], [StreetAddress], [City], [Country], [PostalCode])
			VALUES
			(@jobAdvertVacantJobId, @streetAddress, @city, @country, @postalCode);
		
			COMMIT TRANSACTION @TranName;			
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error while creating Address', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH

GO

CREATE PROCEDURE [JA.spUpdateAddress](
	@jobAdvertVacantJobId int,
	@streetAddress varchar(250),
	@city varchar(100),
	@country varchar(100),
	@postalCode varchar(10))
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'AddressUpdate';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Updating an Address';

	BEGIN TRY
		IF NOT EXISTS (SELECT * FROM [Address] WHERE [JobAdvertVacantJobId] = @jobAdvertVacantJobId)
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Update Address failed, does not exists', 4, 2
		 ELSE
			UPDATE [Address]
			SET
			[StreetAddress] = @streetAddress,
			[City] = @city,
			[Country] = @country,
			[PostalCode] = @postalCode
			WHERE [JobAdvertVacantJobId] = @jobAdvertVacantJobId;

			COMMIT TRANSACTION @TranName;			
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error while updating Address', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

CREATE PROCEDURE [JA.spRemoveAddress](
	@jobAdvertVacantJobId int)
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'AddressRemove';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Removing an Address';

	BEGIN TRY
		IF NOT EXISTS (SELECT * FROM [Address] WHERE [JobAdvertVacantJobId] = @jobAdvertVacantJobId)
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Removing Address failed, does not exists', 4, 2
		 ELSE
			DELETE FROM [Address] WHERE [JobAdvertVacantJobId] = @jobAdvertVacantJobId;

			COMMIT TRANSACTION @TranName;			
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error while removing Address', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

CREATE PROCEDURE [JA.spGetAddressById](
	@jobAdvertVacantJobId int)
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'AddressGetById';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Getting an Address by ID';

	BEGIN TRY
		IF NOT EXISTS (SELECT * FROM [Address] WHERE [JobAdvertVacantJobId] = @jobAdvertVacantJobId)
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Getting Address failed, does not exists', 4, 2
		 ELSE
			SELECT 
			StreetAddress AS 'Street Address', 
			City AS 'City', 
			Country AS 'Country', 
			PostalCode AS 'Postal Code' 
			FROM [Address] 
			WHERE [JobAdvertVacantJobId] = @jobAdvertVacantJobId;

			COMMIT TRANSACTION @TranName;			
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error while getting Address by ID', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

CREATE PROCEDURE [JA.spGetAddresses]
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'GetAddresses';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Getting Address collection';

	BEGIN TRY
		IF NOT EXISTS (SELECT * FROM [Address])
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Getting Address collection failed, was empty', 4, 2
		 ELSE
			SELECT 
			StreetAddress AS 'Street Address', 
			City AS 'City', 
			Country AS 'Country', 
			PostalCode AS 'Postal Code' 
			FROM [Address];

			COMMIT TRANSACTION @TranName;			
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error while getting Address collection', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

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

CREATE PROCEDURE [JA.spCreateUser](
	@userId int,
	@roleId int,
	@locationId int,
	@userFirstName varchar(50),
	@userLastName varchar(50),
	@userEmail varchar(50),
	@userPass varchar(1000),
	@userSalt varchar(1000),
	@userAccessToken varchar(max))
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'UserInsert';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Inserting a User';

	BEGIN TRY
		IF EXISTS (SELECT * FROM [User] WHERE [Id] = @userId)
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Creating User failed, already exists', 4, 2
		 ELSE
			INSERT INTO [User] ([Id], [RoleId], [LocationId], [FirstName], [LastName], [Email], [Password], [Salt],[AccessToken])
			VALUES
			(@userId, @roleId, @locationId, @userFirstName, @userLastName, @userEmail, @userPass, @userSalt, @userAccessToken);
		
			COMMIT TRANSACTION @TranName;			
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error while creating User', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH

GO

CREATE PROCEDURE [JA.spUpdateUser](
	@userId int,
	@roleId int,
	@locationId int,
	@userFirstName varchar(50),
	@userLastName varchar(50),
	@userEmail varchar(50),
	@userPass varchar(1000),
	@userSalt varchar(1000),
	@userAccessToken varchar(max))
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'UserUpdate';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Updating a User';

	BEGIN TRY
		IF NOT EXISTS (SELECT * FROM [User] WHERE [Id] = @userId)
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Updating User failed, does not exists', 4, 2
		 ELSE
			UPDATE [User]
			SET
			[RoleId] = @roleId,
			[LocationId] = @locationId,
			[FirstName] = @userFirstName,
			[LastName] = @userLastName,
			[Email] = @userEmail,
			[Password] = @userPass,
			[salt] = @userSalt,
			[AccessToken] = @userAccessToken
			WHERE [Id] = @userId;
		
			COMMIT TRANSACTION @TranName;			
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error while creating User', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH

GO

CREATE PROCEDURE [JA.spRemoveUser](
	@userId int)
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'UserRemove';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Removing a User';

	BEGIN TRY
		IF NOT EXISTS (SELECT * FROM [User] WHERE [Id] = @userId)
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Removing User failed, does not exists', 4, 2
		 ELSE
			DELETE FROM [Contract] WHERE [UserId] = @userId;
			DELETE FROM [ConsultantArea] WHERE [UserId] = @userId;
			DELETE FROM [User] WHERE [Id] = @userId;

			COMMIT TRANSACTION @TranName;			
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error while removing User', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

CREATE PROCEDURE [JA.spGetUserById](
	@userId int)
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'UserGetById';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Getting a User by ID';

	BEGIN TRY
		IF NOT EXISTS (SELECT * FROM [User] WHERE [Id] = @userId)
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Getting User failed, does not exists', 4, 2
		 ELSE
			SELECT 
			u.[Id] AS 'User ID',
			u.[FirstName] AS 'User Firstname',
			u.[LastName] AS 'User Lastname',
			u.[Email] AS 'User Email',

			r.[Name] AS 'User Role',
			l.[Name] AS 'User Location',

			STUFF((	SELECT '; ' + a.[Name]
					FROM [ConsultantArea] c
						INNER JOIN [Area] a ON a.[Id] = c.[AreaId]
					WHERE c.[UserId] = 1), 1, 1, '') AS 'Consultant Areas'

			FROM [User] u
				INNER JOIN [Role] r ON r.[Id] = u.[RoleId]
				INNER JOIN [Location] l ON l.[Id] = u.[LocationId]
				INNER JOIN [ConsultantArea] c ON c.[UserId] = u.[Id]
				INNER JOIN [Area] a ON a.[Id] = c.[AreaId]
			WHERE u.[Id] = @userId

			COMMIT TRANSACTION @TranName;			
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error while getting User by ID', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

CREATE PROCEDURE [JA.spGetUsers](
	@userId int)
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'UserGetAll';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Getting a collection of User';

	BEGIN TRY
		IF NOT EXISTS (SELECT * FROM [User])
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Getting collection of User failed, does not exists', 4, 2
		 ELSE
			SELECT 
			u.[Id] AS 'User ID',
			u.[FirstName] AS 'User Firstname',
			u.[LastName] AS 'User Lastname',
			u.[Email] AS 'User Email',

			r.[Name] AS 'User Role',
			l.[Name] AS 'User Location',

			STUFF((	SELECT '; ' + a.[Name]
					FROM [ConsultantArea] c
						INNER JOIN [Area] a ON a.[Id] = c.[AreaId]
					WHERE c.[UserId] = 1), 1, 1, '') AS 'Consultant Areas'

			FROM [User] u
				INNER JOIN [Role] r ON r.[Id] = u.[RoleId]
				INNER JOIN [Location] l ON l.[Id] = u.[LocationId]
				INNER JOIN [ConsultantArea] c ON c.[UserId] = u.[Id]
				INNER JOIN [Area] a ON a.[Id] = c.[AreaId]

			COMMIT TRANSACTION @TranName;			
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error while getting collection of User', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH
GO

CREATE PROCEDURE [JA.spGrantUserArea](
	@userId int,
	@areaId int)
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'UserGrantArea';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Granting an area to User';

	BEGIN TRY
		IF NOT EXISTS (SELECT * FROM [User] WHERE [Id] = @userId) OR NOT EXISTS (SELECT * FROM [Area] WHERE [Id] = @areaId)
				EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Granting area to User failed, user or area does not exists', 4, 2
		ELSE
			IF EXISTS (SELECT * FROM [ConsultantArea] WHERE [UserId] = @userId AND [AreaId] = @areaId)
				EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Granting area to user failed, user already this area.', 4, 2
			ELSE
				INSERT INTO [ConsultantArea] ([UserId], [AreaId])
				VALUES (@userId, @areaId);
			
			COMMIT TRANSACTION @TranName;
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error while granting an area to User', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH

GO

CREATE PROCEDURE [JA.spRemoveUserArea](
	@userId int,
	@areaId int)
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'UserRemoveArea';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Removing an area from User';

	BEGIN TRY
		IF NOT EXISTS (SELECT * FROM [User] WHERE [Id] = @userId) OR NOT EXISTS (SELECT * FROM [Area] WHERE [Id] = @areaId)
				EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Removing area from User failed, user or area does not exists', 4, 2
		ELSE
			IF NOT EXISTS (SELECT * FROM [ConsultantArea] WHERE [UserId] = @userId AND [AreaId] = @areaId)
				EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Removing area from user failed, user does not have this area.', 4, 2
			ELSE
				DELETE FROM [ConsultantArea] WHERE [UserId] = @userId AND [AreaId] = @areaId;
			
			COMMIT TRANSACTION @TranName;
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error while removing an area from User', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH

GO

CREATE PROCEDURE [JA.spGetUserSaltByEmail](
	@userEmail varchar(50))
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'UserGetSaltByEmail';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Getting a User salt by email';

	BEGIN TRY
		IF NOT EXISTS (SELECT * FROM [User] WHERE [Email] = @userEmail)
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Getting User salt by Email failed, user does not exists', 4, 2
		ELSE
			SELECT 
			[Salt] AS 'User Salt'
			FROM [User]
			WHERE [Email] = @userEmail

			COMMIT TRANSACTION @TranName;
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error while getting User salt by email', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH

GO

CREATE PROCEDURE [JA.spGetUserByAccessToken](
	@userAccessToken varchar(max))
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'UserGetByAccessToken';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Getting a User by AccessToken';

	BEGIN TRY
		IF NOT EXISTS (SELECT * FROM [User] WHERE [AccessToken] = @userAccessToken)
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Failed getting User by AccessToken, user does not exists', 4, 2
		ELSE
			SELECT 
			u.[Id] AS 'User ID',
			u.[FirstName] AS 'User Firstname',
			u.[LastName] AS 'User Lastname',
			u.[Email] AS 'User Email',

			r.[Name] AS 'User Role',
			l.[Name] AS 'User Location',

			STUFF((	SELECT '; ' + a.[Name]
					FROM [ConsultantArea] c
						INNER JOIN [Area] a ON a.[Id] = c.[AreaId]
					WHERE c.[UserId] = 1), 1, 1, '') AS 'Consultant Areas'

			FROM [User] u
				INNER JOIN [Role] r ON r.[Id] = u.[RoleId]
				INNER JOIN [Location] l ON l.[Id] = u.[LocationId]
				INNER JOIN [ConsultantArea] c ON c.[UserId] = u.[Id]
				INNER JOIN [Area] a ON a.[Id] = c.[AreaId]
			WHERE u.[AccessToken] = @userAccessToken

			COMMIT TRANSACTION @TranName;
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error while getting User by access token', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH

GO

CREATE PROCEDURE [JA.spUpdateUserSecurity](
	@userId int,
	@userNewPassword varchar(1000),
	@userOldPassword varchar(1000),
	@userNewSalt varchar(1000),
	@resultReturn bit = 0 OUTPUT)
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'UserUpdateSecurity';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Updates security info of User';

	BEGIN TRY
		IF NOT EXISTS (SELECT * FROM [User] WHERE [Id] = @userId)
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Failed updating User security, user does not exists', 4, 2 
		ELSE
			IF NOT EXISTS (SELECT * FROM [User] WHERE [Password] = @userOldPassword)
				EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Failed updating User security, password does not match', 4, 2 
			ELSE
				UPDATE [User]
				SET
				[Password] = @userNewPassword,
				[Salt] = @userNewSalt
				WHERE [Id] = @userId;

				SET @resultReturn = 1;
				COMMIT TRANSACTION @TranName;
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error while updating User security', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH

GO

CREATE PROCEDURE [JA.spValidateUserExists](
	@userEmail varchar(50),
	@returnResult bit = 0 OUTPUT)
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'UserValidateExists';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Validates the existance of a User';

	BEGIN TRY
		IF NOT EXISTS (SELECT * FROM [User] WHERE [Email] = @userEmail)
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Failed validating existence of User, user does not exists', 4, 2
		ELSE
			SET @returnResult = 1;


			COMMIT TRANSACTION @TranName;
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error while validating existence of User', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH

GO

CREATE PROCEDURE [JA.spValidateUserLogin](
	@userEmail varchar(50),
	@userPassword varchar(1000),
	@returnResult bit = 0 OUTPUT)
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'UserValidateLogin';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Validating a User login';

	BEGIN TRY
		IF NOT EXISTS (SELECT * FROM [User] WHERE [Email] = @userEmail AND [Password] = @userPassword)
			EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Failed validating User login, credentials does not match', 4, 2
		ELSE
			SET @returnResult = 1;


			COMMIT TRANSACTION @TranName;
	END TRY
	BEGIN CATCH
		EXEC [JobAgentLogDB].[dbo].[JA.log.spCreateLog] @TranName, 'Error while validating User login', 6, 2

		ROLLBACK TRANSACTION @TranName
	END CATCH

GO
