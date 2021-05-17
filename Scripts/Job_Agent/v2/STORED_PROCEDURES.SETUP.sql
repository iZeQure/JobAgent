USE [JobAgentDB_v2]
GO

/*##################################################
				## Area Section ##
####################################################*/

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
		INSERT INTO [Area] ([Name])
		VALUES
		(@areaName);

		COMMIT TRANSACTION @TranName;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
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
		UPDATE [Area]
			SET [Name] = @areaName
		WHERE [Area].[Id] = @areaId;

		COMMIT TRANSACTION @TranName;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
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
		-- Remove all references to the primary key.
		DELETE FROM [ConsultantArea] WHERE [AreaId] = @areaId;
		DELETE FROM [Area] WHERE [Id] = @areaId;

		COMMIT TRANSACTION @TranName;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
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
		SELECT
			[dbo].[Area].[Id] AS 'Area ID',
			[dbo].[Area].[Name] as 'Area Name'
		FROM [Area]
		WHERE [Id] = @areaId;

		COMMIT TRANSACTION @TranName;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
	END CATCH
GO

CREATE PROCEDURE [JA.spGetAreas]
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'AreaGetAll';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Get collection of area';

	BEGIN TRY
		SELECT
			[dbo].[Area].[Id] AS 'Area ID',
			[dbo].[Area].[Name] as 'Area Name'
		FROM [Area];

		COMMIT TRANSACTION @TranName;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
	END CATCH
GO

/*##################################################
				## Role Section ##
####################################################*/

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
		INSERT INTO [Role] ([Name], [Description])
		VALUES
		(@roleName, @roleDesription);

		COMMIT TRANSACTION @TranName;			
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
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
		UPDATE [Role]
			SET [Name] = @roleName,
				[Description] = @roleDescription
		WHERE [Role].[Id] = @roleId;

		COMMIT TRANSACTION @TranName;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
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
		DELETE FROM [Role]
		WHERE [Role].[Id] = @roleId;

		COMMIT TRANSACTION @TranName;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
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
		SELECT
			[dbo].[Role].[Id] AS 'Role ID',
			[dbo].[Role].[Name] AS 'Role Name',
			[dbo].[Role].[Description] AS 'Role Description'
		FROM [Role]
		WHERE [Id] = @roleId;

		COMMIT TRANSACTION @TranName;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
	END CATCH
GO

CREATE PROCEDURE [JA.spGetRoles]
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'RoleGetAll';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Get collection of Role';

	BEGIN TRY
		SELECT
			[dbo].[Role].[Id] AS 'Role ID',
			[dbo].[Role].[Name] AS 'Role Name',
			[dbo].[Role].[Description] AS 'Role Description'
		FROM [Role];

		COMMIT TRANSACTION @TranName;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
	END CATCH
GO

/*##################################################
				## Location Section ##
####################################################*/

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
		INSERT INTO [Location] ([Name])
		VALUES
		(@locationName);

		COMMIT TRANSACTION @TranName;			
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
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
		UPDATE [Location]
			SET [Name] = @locationName
		WHERE [dbo].[Location].[Id] = @locationId;

		COMMIT TRANSACTION @TranName;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
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
		DELETE FROM [Location]
		WHERE [Location].[Id] = @locationId;

		COMMIT TRANSACTION @TranName;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
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
		SELECT
			[dbo].[Location].[Id] AS 'Location ID',
			[dbo].[Location].[Name] AS 'Location Name'
		FROM [Location]
		WHERE [Id] = @locationId;

		COMMIT TRANSACTION @TranName;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
	END CATCH
GO

CREATE PROCEDURE [JA.spGetLocations]
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'LocationGetAll';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Get collection of Location';

	BEGIN TRY
		SELECT
			[dbo].[Location].[Id] AS 'Location ID',
			[dbo].[Location].[Name] AS 'Location Name'
		FROM [Location];

		COMMIT TRANSACTION @TranName;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
	END CATCH
GO

/*##################################################
				## Contract Section ##
####################################################*/

DROP PROCEDURE IF EXISTS [JA.spCreateContract]
DROP PROCEDURE IF EXISTS [JA.spUpdateContract]
DROP PROCEDURE IF EXISTS [JA.spRemoveContract]
DROP PROCEDURE IF EXISTS [JA.spGetContractById]
DROP PROCEDURE IF EXISTS [JA.spGetContracts]
GO

CREATE PROCEDURE [JA.spCreateContract] (
	@companyId int,
	@userId int,
	@name uniqueidentifier,
	@regiDateTime datetime = NULL,
	@expDateTime datetime = NULL)
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'ContractInsert';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Inserting a Contract';

	BEGIN TRY
		DECLARE @registration DATETIME = GETDATE(), 
				@expiry DATETIME = DATEADD(YEAR,5,GETDATE())

		SET @registration = IIF (@regiDateTime IS NULL, GETDATE(), @regiDateTime);
		SET @expiry = IIF (@expDateTime IS NULL, DATEADD(YEAR, 5, @registration), @expDateTime);

		INSERT INTO [Contract] ([CompanyId], [UserId], [Name], [RegistrationDateTime], [ExpiryDateTime])
		VALUES
		(@companyId, @userId, @name, @registration, @expiry);

		COMMIT TRANSACTION @TranName;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
	END CATCH
GO

CREATE PROCEDURE [JA.spUpdateContract] (
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
		UPDATE [Contract]
			SET 
				[UserId] = @userId,
				[Name] = @name,
				[RegistrationDateTime] = @registrationDateTime,
				[ExpiryDateTime] = @expiryDateTime
		WHERE [Contract].[CompanyId] = @companyId;

		COMMIT TRANSACTION @TranName;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
	END CATCH
GO

CREATE PROCEDURE [JA.spRemoveContract] (
	@companyId int)
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'ContractRemove';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Removing a Contract';

	BEGIN TRY
		DELETE FROM [Contract]
		WHERE [CompanyId] = @companyId;

		COMMIT TRANSACTION @TranName;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
	END CATCH
GO

CREATE PROCEDURE [JA.spGetContractById] (
	@companyId int)
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'ContractGetById';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Getting a contract by id';

	BEGIN TRY
		SELECT
			[dbo].[Contract].[CompanyId] AS 'Contract ID',
			[dbo].[Contract].[UserId] AS 'User ID',
			[dbo].[Contract].[Name] AS 'Contract Name',
			[dbo].[Contract].[RegistrationDateTime] AS 'Registered Date',
			[dbo].[Contract].[ExpiryDateTime] AS 'Expiration Date'
		FROM [Contract]
		WHERE [CompanyId] = @companyId;

		COMMIT TRANSACTION @TranName;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
	END CATCH
GO

CREATE PROCEDURE [JA.spGetContracts]
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'ContractGetAll';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Get collection of contract';

	BEGIN TRY
		SELECT
			[dbo].[Contract].[CompanyId] AS 'Contract ID',
			[dbo].[Contract].[UserId] AS 'User ID',
			[dbo].[Contract].[Name] AS 'Contract Name',
			[dbo].[Contract].[RegistrationDateTime] AS 'Registered Date',
			[dbo].[Contract].[ExpiryDateTime] AS 'Expiration Date'
		FROM [Contract];

		COMMIT TRANSACTION @TranName;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
	END CATCH
GO

/*##################################################
				## Company Section ##
####################################################*/

DROP PROCEDURE IF EXISTS [JA.spCreateCompany]
DROP PROCEDURE IF EXISTS [JA.spUpdateCompany]
DROP PROCEDURE IF EXISTS [JA.spRemoveCompany]
DROP PROCEDURE IF EXISTS [JA.spGetCompanyById]
DROP PROCEDURE IF EXISTS [JA.spGetCompanies]
GO

CREATE PROCEDURE [JA.spCreateCompany] (
	@companyCVR int,
	@companyName varchar(50),
	@contactPerson varchar(50))
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'CompanyInsert';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Inserting a Company';

	BEGIN TRY
		INSERT INTO [Company] ([CVR], [Name], [ContactPerson])
		VALUES
		(@CompanyCVR, @companyname, @contactPerson);

		COMMIT TRANSACTION @TranName;			
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
	END CATCH
GO

CREATE PROCEDURE [JA.spUpdateCompany] (
    @companyId int,
	@companyCVR int,
	@companyName varchar(50),
	@contactPerson varchar(50))
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'CompanyUpdate';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Updating a Company';

	BEGIN TRY
		UPDATE [Company]
			SET [CVR] = @companyCVR,
				[Name] = @companyName,
				[ContactPerson] = @contactPerson
		WHERE [Company].[CVR] = @companyCVR;

		COMMIT TRANSACTION @TranName;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
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
		DECLARE @vacantJobId int
		SET @vacantJobId = (SELECT [Id] FROM [VacantJob] WHERE [CompanyId] = @companyId)

		-- Remove all references to the primary key.
		DELETE FROM [JobAdvert] WHERE [VacantJobId] = @vacantJobId;
		DELETE FROM [VacantJob] WHERE [CompanyId] = @companyId;
		DELETE FROM [Contract] WHERE [CompanyId] = @companyId;
		DELETE FROM [Company] WHERE [Id] = @companyId;

		COMMIT TRANSACTION @TranName;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
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
		SELECT
			[dbo].[Company].[CVR] AS 'Company CVR',
			[dbo].[Company].[Name] AS 'Company Name',
			[dbo].[Company].[ContactPerson] AS 'Contact Person'
		FROM [Company]
		WHERE [Id] = @companyId;

		COMMIT TRANSACTION @TranName;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
	END CATCH
GO

CREATE PROCEDURE [JA.spGetCompanies]
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'CompanyGetAll';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Get collection of Company';

	BEGIN TRY
		SELECT
			[dbo].[Company].[Id] AS 'ID',
			[dbo].[Company].[CVR] AS 'Company CVR',
			[dbo].[Company].[Name] as 'Company Name',
			[dbo].[Company].[ContactPerson] as 'Contact person'
		FROM [Company];

		COMMIT TRANSACTION @TranName;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
	END CATCH
GO

/*##################################################
			## Vacant Job Section ##
####################################################*/

DROP PROCEDURE IF EXISTS [JA.spCreateVacantJob]
DROP PROCEDURE IF EXISTS [JA.spUpdateVacantJob]
DROP PROCEDURE IF EXISTS [JA.spRemoveVacantJob]
DROP PROCEDURE IF EXISTS [JA.spGetVacantJobById]
DROP PROCEDURE IF EXISTS [JA.spGetVacantJobs]
GO

CREATE PROCEDURE [JA.spCreateVacantJob] (
	@companyId int,
	@url varchar(2048))
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'VacantJobInsert';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Inserting a VacantJob';

	BEGIN TRY
		INSERT INTO [VacantJob] ([URL], [CompanyId])
		VALUES
		(@url, @companyId);

		COMMIT TRANSACTION @TranName;			
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
	END CATCH
GO

CREATE PROCEDURE [JA.spUpdateVacantJob] (
	@vacantJobId int,
	@companyId int,
	@url varchar(2048))
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'VacantJobUpdate';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Updating a VacantJob';

	BEGIN TRY
		UPDATE [VacantJob]
			SET [URL] = @url,
				[CompanyId] = @companyId
		WHERE [VacantJob].[Id] = @vacantJobId;

		COMMIT TRANSACTION @TranName;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
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
		-- Remove all references to the primary key.
		DELETE FROM [Address] WHERE [JobAdvertVacantJobId] = @vacantJobId;
		DELETE FROM [JobAdvert] WHERE [VacantJobId] = @vacantJobId;
		DELETE FROM [VacantJob] WHERE [Id] = @vacantJobId;

		COMMIT TRANSACTION @TranName;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
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
		SELECT
			[dbo].[VacantJob].[Id] AS 'Vacant job ID',
			[dbo].[VacantJob].[URL] AS 'VacantJob URL',
			[dbo].[VacantJob].[CompanyId] AS 'Company ID'				
		FROM [VacantJob]
		WHERE [Id] = @vacantJobId;

		COMMIT TRANSACTION @TranName;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
	END CATCH
GO

CREATE PROCEDURE [JA.spGetVacantJobs]
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'VacantJobGetAll';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Get collection of VacantJob';

	BEGIN TRY
		SELECT
			[dbo].[VacantJob].[Id] AS 'Vacant job ID',
			[dbo].[VacantJob].[URL] AS 'VacantJob URL',
			[dbo].[VacantJob].[CompanyId] AS 'Company ID'
		FROM [VacantJob]

		COMMIT TRANSACTION @TranName
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
	END CATCH
GO

/*##################################################
				## Category Section ##
####################################################*/

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
		INSERT INTO [Category] ([Name])
		VALUES
		(@categoryName);

		COMMIT TRANSACTION @TranName;			
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
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
		UPDATE [Category]
			SET [Name] = @categoryName
		WHERE [Category].[Id] = @categoryId;

		COMMIT TRANSACTION @TranName;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
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
		-- Remove all references to the primary key.
		DELETE FROM [JobAdvert] WHERE [CategoryId] = @categoryId;
		DELETE FROM [Category]	WHERE [Id] = @categoryId;

		COMMIT TRANSACTION @TranName;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
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
		SELECT
			[dbo].[Category].[Id] AS 'Category ID',
			[dbo].[Category].[Name] AS 'Category Name'
		FROM [Category]
		WHERE [Id] = @categoryId;

		COMMIT TRANSACTION @TranName;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
	END CATCH
GO

CREATE PROCEDURE [JA.spGetCategories]
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'CategoryGetAll';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Get collection of Category';

	BEGIN TRY
		SELECT
			[dbo].[Category].[Id] AS 'Category ID',
			[dbo].[Category].[Name] AS 'Category Name'
		FROM [Category];

		COMMIT TRANSACTION @TranName;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
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
		LEFT OUTER JOIN [Specialization] ON [Category].[Id] = [Specialization].[CategoryId];

		COMMIT TRANSACTION @TranName;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
	END CATCH
GO

/*##################################################
			## Specialization Section ##
####################################################*/

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
		INSERT INTO [Specialization] ([Name], [CategoryId])
		VALUES
		(@specializationName, @categoryId);

		COMMIT TRANSACTION @TranName;			
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
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
		UPDATE [Specialization]
			SET [Name] = @specializationName,
				[CategoryId] = @categoryId
			WHERE [Id] = @specializationId;

		COMMIT TRANSACTION @TranName;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
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
		DELETE FROM [JobAdvert] WHERE [SpecializationId] = @specializationId;
		DELETE FROM [Specialization] WHERE [Id] = @specializationId;

		COMMIT TRANSACTION @TranName;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
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
		SELECT
			[dbo].[Specialization].[Id] AS 'Specialization ID',
			[dbo].[Specialization].[Name] AS 'Specialization Name',
			[dbo].[Specialization].[CategoryId] AS 'Category ID'
		FROM
			[Specialization] WHERE [Id] = @specializationId;

		COMMIT TRANSACTION @TranName;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
	END CATCH
GO

CREATE PROCEDURE [JA.spGetSpecializations]
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'SpecializationGetAll';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Get collection of Specialization';

	BEGIN TRY
		SELECT
			[dbo].[Specialization].[Id] AS 'Specialization ID',
			[dbo].[Specialization].[Name] AS 'Specialization Name',
			[dbo].[Specialization].[CategoryId] AS 'Category ID'
		FROM
			[Specialization];

		COMMIT TRANSACTION @TranName;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
	END CATCH
GO

/*##################################################
			  ## Job Advert Section ##
####################################################*/

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
	@categoryId int,
    @specializationId int,
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
		INSERT INTO [JobAdvert] ([VacantJobId], [CategoryId], [SpecializationId], [Title], [Summary], [Description], [Email], [PhoneNumber], [RegistrationDateTime], [ApplicationDeadlineDateTime])
		VALUES
		(@vacantJobId, @specializationId, @categoryId, @jobAdvertTitle, @jobAdvertSummary, @jobAdvertDescription, @jobAdvertEmail, @jobAdvertPhoneNr, @jobAdvertRegistrationDateTime, @jobAdvertApplicationDeadlineDateTime);

		COMMIT TRANSACTION @TranName;			
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
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
			WHERE [VacantJobId] = @vacantJobId;

		COMMIT TRANSACTION @TranName;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
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
		DELETE FROM [Address] WHERE [JobAdvertVacantJobId] = @vacantJobId;
		DELETE FROM [JobAdvert] WHERE [VacantJobId] = @vacantJobId;

		COMMIT TRANSACTION @TranName;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
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
			[JobAdvert] j WHERE [VacantJobId] = @vacantJobId;

		COMMIT TRANSACTION @TranName;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
	END CATCH
GO

CREATE PROCEDURE [JA.spGetJobAdverts]
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'JobAdvertGetAll';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Get collection of JobAdvert';

	BEGIN TRY
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
			[JobAdvert] j;

		COMMIT TRANSACTION @TranName;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
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
		SET @countByCategory = (SELECT COUNT (*) FROM [JobAdvert] WHERE [CategoryId] = @categoryId);

		COMMIT TRANSACTION @TranName;
		
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
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
		SET @countBySpecialization = (SELECT COUNT (*) FROM [JobAdvert] WHERE [SpecializationId] = @specializationId);

		COMMIT TRANSACTION @TranName
		
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
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
		SET @countByNonCategorized = (SELECT COUNT (*) FROM [JobAdvert] WHERE [CategoryId] = 0 AND [SpecializationId] = 0);

			COMMIT TRANSACTION @TranName;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
	END CATCH
GO

/*##################################################
				## Address Section ##
####################################################*/

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
		INSERT INTO [Address] ([JobAdvertVacantJobId], [StreetAddress], [City], [Country], [PostalCode])
		VALUES
		(@jobAdvertVacantJobId, @streetAddress, @city, @country, @postalCode);
		
		COMMIT TRANSACTION @TranName;			
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
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
		ROLLBACK TRANSACTION @TranName;
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
		DELETE FROM [Address] WHERE [JobAdvertVacantJobId] = @jobAdvertVacantJobId;

		COMMIT TRANSACTION @TranName;			
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
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
		ROLLBACK TRANSACTION @TranName;
	END CATCH
GO

CREATE PROCEDURE [JA.spGetAddresses]
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'GetAddresses';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Getting Address collection';

	BEGIN TRY
		SELECT 
		StreetAddress AS 'Street Address', 
		City AS 'City', 
		Country AS 'Country', 
		PostalCode AS 'Postal Code'
		FROM [Address];

		COMMIT TRANSACTION @TranName;			
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
	END CATCH
GO

/*##################################################
				## User Section ##
####################################################*/

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
		INSERT INTO [User] ([RoleId], [LocationId], [FirstName], [LastName], [Email], [Password], [Salt],[AccessToken])
		VALUES
		(@roleId, @locationId, @userFirstName, @userLastName, @userEmail, @userPass, @userSalt, @userAccessToken);
		
		COMMIT TRANSACTION @TranName;			
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
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
		ROLLBACK TRANSACTION @TranName;
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
		DELETE FROM [Contract] WHERE [UserId] = @userId;
		DELETE FROM [ConsultantArea] WHERE [UserId] = @userId;
		DELETE FROM [User] WHERE [Id] = @userId;

		COMMIT TRANSACTION @TranName;			
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
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
		ROLLBACK TRANSACTION @TranName;
	END CATCH
GO

CREATE PROCEDURE [JA.spGetUsers]
AS
	DECLARE @TranName varchar(20)
	SELECT @TranName = 'UserGetAll';

	BEGIN TRANSACTION @TranName
		WITH MARK N'Getting a collection of User';

	BEGIN TRY
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
			INNER JOIN [Area] a ON a.[Id] = c.[AreaId];

		COMMIT TRANSACTION @TranName;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
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
		INSERT INTO [ConsultantArea] ([UserId], [AreaId])
		VALUES (@userId, @areaId);
			
		COMMIT TRANSACTION @TranName;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
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
			DELETE FROM [ConsultantArea] WHERE [UserId] = @userId AND [AreaId] = @areaId;
			
			COMMIT TRANSACTION @TranName;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
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
		SELECT 
			[Salt] AS 'User Salt'
		FROM [User]
		WHERE [Email] = @userEmail;

		COMMIT TRANSACTION @TranName;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
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
		WHERE u.[AccessToken] = @userAccessToken;

		COMMIT TRANSACTION @TranName;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
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
		UPDATE [User]
			SET
			[Password] = @userNewPassword,
			[Salt] = @userNewSalt
		WHERE [Id] = @userId;

		SET @resultReturn = 1;
		COMMIT TRANSACTION @TranName;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
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
		SET @returnResult = 1;
		COMMIT TRANSACTION @TranName;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName;
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
		SET @returnResult = 1;
		COMMIT TRANSACTION @TranName;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TranName; 
	END CATCH
GO
