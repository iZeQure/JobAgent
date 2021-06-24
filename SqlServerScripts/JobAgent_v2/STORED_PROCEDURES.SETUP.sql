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
DECLARE @TName varchar(20) = 'AreaCreateTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		INSERT INTO [Area] ([Name])
		VALUES
		(@areaName);

		COMMIT TRAN @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		  BEGIN
			ROLLBACK TRAN @TName;
		  END 
	END CATCH
END;	
GO

CREATE PROCEDURE [JA.spUpdateArea] (
	@areaId int,
	@areaName varchar(50))
AS
DECLARE @TName varchar(20) = 'AreaUpdateTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		UPDATE [Area]
			SET [Name] = @areaName
		WHERE [Area].[Id] = @areaId;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @TName;
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spRemoveArea] (
	@areaId int)
AS
DECLARE @TName varchar(20) = 'AreaRemoveTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		-- Remove all references to the primary key.
		DELETE FROM [ConsultantArea] WHERE [AreaId] = @areaId;
		DELETE FROM [Area] WHERE [Id] = @areaId;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spGetAreaById] (
	@areaId int)
AS
DECLARE @TName varchar(20) = 'AreaGetByIdTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		SELECT
			a.[Id] AS 'Area ID',
			a.[Name] as 'Area Name'
		FROM [Area] a
		WHERE [Id] = @areaId;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spGetAreas]
AS
DECLARE @TName varchar(20) = 'AreaGetAllTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		SELECT
			[dbo].[Area].[Id] AS 'Area ID',
			[dbo].[Area].[Name] as 'Area Name'
		FROM [Area];

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
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
DECLARE @TName varchar(20) = 'RoleCreateTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		INSERT INTO [Role] ([Name], [Description])
		VALUES
		(@roleName, @roleDesription);

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spUpdateRole] (
	@roleId int,
	@roleName varchar(30),
	@roleDescription varchar(100))
AS
DECLARE @TName varchar(20) = 'RoleUpdateTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		UPDATE [Role]
			SET [Name] = @roleName,
				[Description] = @roleDescription
		WHERE [Role].[Id] = @roleId;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spRemoveRole] (
	@roleId int)
AS
DECLARE @TName varchar(20) = 'RoleRemoveTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		DELETE FROM [Role]
		WHERE [Role].[Id] = @roleId;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spGetRoleById] (
	@roleId int)
AS
DECLARE @TName varchar(20) = 'RoleGetByIdTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		SELECT
			r.[Id] AS 'Role ID',
			r.[Name] AS 'Role Name',
			r.[Description] AS 'Role Description'
		FROM [Role] r
		WHERE [Id] = @roleId;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spGetRoles]
AS
DECLARE @TName varchar(20) = 'RoleGetAllTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		SELECT
			r.[Id] AS 'Role ID',
			r.[Name] AS 'Role Name',
			r.[Description] AS 'Role Description'
		FROM [Role] r;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
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
DECLARE @TName varchar(20) = 'LocationCreateTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		INSERT INTO [Location] ([Name])
		VALUES
		(@locationName);

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spUpdateLocation] (
	@locationId int,
	@locationName varchar(30))
AS
DECLARE @TName varchar(20) = 'LocationUpdateTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		UPDATE [Location]
			SET [Name] = @locationName
		WHERE [dbo].[Location].[Id] = @locationId;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spRemoveLocation] (
	@locationId int)
AS
DECLARE @TName varchar(20) = 'LocationRemoveTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		DELETE FROM [Location]
		WHERE [Location].[Id] = @locationId;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spGetLocationById] (
	@locationId int)
AS
DECLARE @TName varchar(20) = 'LocationGetByIdTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		SELECT
			l.[Id] AS 'Location ID',
			l.[Name] AS 'Location Name'
		FROM [Location] l
		WHERE l.[Id] = @locationId;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spGetLocations]
AS
DECLARE @TName varchar(20) = 'LocationGetByIdTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		SELECT
			l.[Id] AS 'Location ID',
			l.[Name] AS 'Location Name'
		FROM [Location] l;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
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
DECLARE @TName varchar(20) = 'ContractCreateTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		DECLARE @registration DATETIME = GETDATE(), 
				@expiry DATETIME = DATEADD(YEAR,5,GETDATE())

		SET @registration = IIF (@regiDateTime IS NULL, GETDATE(), @regiDateTime);
		SET @expiry = IIF (@expDateTime IS NULL, DATEADD(YEAR, 5, @registration), @expDateTime);

		INSERT INTO [Contract] ([CompanyId], [UserId], [Name], [RegistrationDateTime], [ExpiryDateTime])
		VALUES
		(@companyId, @userId, @name, @registration, @expiry);

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spUpdateContract] (
	@companyId int,
	@userId varchar(50),
	@name uniqueidentifier,
	@registrationDateTime datetime,
	@expiryDateTime datetime)
AS
DECLARE @TName varchar(20) = 'ContractUpdateTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		UPDATE [Contract]
			SET 
				[UserId] = @userId,
				[Name] = @name,
				[RegistrationDateTime] = @registrationDateTime,
				[ExpiryDateTime] = @expiryDateTime
		WHERE [Contract].[CompanyId] = @companyId;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spRemoveContract] (
	@companyId int)
AS
DECLARE @TName varchar(20) = 'ContractRemoveTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		DELETE FROM [Contract]
		WHERE [CompanyId] = @companyId;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spGetContractById] (
	@companyId int)
AS
DECLARE @TName varchar(20) = 'ContractGetByIdTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		SELECT
			c.[CompanyId] AS 'Contract ID',
			c.[UserId] AS 'User ID',
			c.[Name] AS 'Contract Name',
			c.[RegistrationDateTime] AS 'Registered Date',
			c.[ExpiryDateTime] AS 'Expiration Date'
		FROM [Contract] c
		WHERE [CompanyId] = @companyId;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spGetContracts]
AS
DECLARE @TName varchar(20) = 'ContractGetAllTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		SELECT
			c.[CompanyId] AS 'Contract ID',
			c.[UserId] AS 'User ID',
			c.[Name] AS 'Contract Name',
			c.[RegistrationDateTime] AS 'Registered Date',
			c.[ExpiryDateTime] AS 'Expiration Date'
		FROM [Contract] c;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
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
DECLARE @TName varchar(20) = 'CompanyCreateTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		INSERT INTO [Company] ([CVR], [Name], [ContactPerson])
		VALUES
		(@companyCVR, @companyname, @contactPerson);

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spUpdateCompany] (
    @companyId int,
	@companyCVR int,
	@companyName varchar(50),
	@contactPerson varchar(50))
AS
DECLARE @TName varchar(20) = 'CompanyUpdateTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		UPDATE [Company]
			SET [CVR] = @companyCVR,
				[Name] = @companyName,
				[ContactPerson] = @contactPerson
		WHERE [Company].[CVR] = @companyCVR;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spRemoveCompany] (
	@companyId int)
AS
DECLARE @TName varchar(20) = 'CompanyRemoveTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		DECLARE @vacantJobId int
		SET @vacantJobId = (SELECT [Id] FROM [VacantJob] WHERE [CompanyId] = @companyId)

		-- Remove all references to the primary key.
		DELETE FROM [JobAdvert] WHERE [VacantJobId] = @vacantJobId;
		DELETE FROM [VacantJob] WHERE [CompanyId] = @companyId;
		DELETE FROM [JobPage] WHERE [CompanyId] = @companyId;
		DELETE FROM [Contract] WHERE [CompanyId] = @companyId;
		DELETE FROM [Company] WHERE [Id] = @companyId;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spGetCompanyById] (
	@companyId int)
AS
DECLARE @TName varchar(20) = 'CompanyGetByIdTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		SELECT
			c.[CVR] AS 'Company CVR',
			c.[Name] AS 'Company Name',
			c.[ContactPerson] AS 'Contact Person'
		FROM [Company] c
		WHERE [Id] = @companyId;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spGetCompanies]
AS
DECLARE @TName varchar(20) = 'CompanyGetAllTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		SELECT
			c.[Id] AS 'ID',
			c.[CVR] AS 'Company CVR',
			c.[Name] as 'Company Name',
			c.[ContactPerson] as 'Contact person'
		FROM [Company] c;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

/*##################################################
			## Job Page Section ##
####################################################*/

DROP PROCEDURE IF EXISTS [JA.spCreateJobPage]
DROP PROCEDURE IF EXISTS [JA.spUpdateJobPage]
DROP PROCEDURE IF EXISTS [JA.spRemoveJobPage]
DROP PROCEDURE IF EXISTS [JA.spGetJobPageById]
DROP PROCEDURE IF EXISTS [JA.spGetJobPages]
GO

CREATE PROCEDURE [JA.spCreateJobPage] (
	@companyId int,
	@url varchar(2048))
AS
DECLARE @TName varchar(20) = 'JobPageCreateTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		INSERT INTO [JobPage] ([URL], [CompanyId])
		VALUES
		(@url, @companyId);

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spUpdateJobPage] (
	@jobPageId int,
	@companyId int,
	@url varchar(2048))
AS
DECLARE @TName varchar(20) = 'JobPageUpdateTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		UPDATE [JobPage]
			SET [URL] = @url,
				[CompanyId] = @companyId
		WHERE [JobPage].[Id] = @jobPageId;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spRemoveJobPage] (
	@jobPageId int)
AS
DECLARE @TName varchar(20) = 'JobPageRemoveTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		DELETE FROM [JobPage] WHERE [Id] = @jobPageId;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spGetJobPageById] (
	@jobPageId int)
AS
DECLARE @TName varchar(20) = 'JobPageGetbyIdTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		SELECT
			j.[Id] AS 'JobPage ID',
			j.[URL] AS 'JobPage URL',
			j.[CompanyId] AS 'Company ID'				
		FROM [JobPage] j
		WHERE [Id] = @jobPageId;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spGetJobPages]
AS
DECLARE @TName varchar(20) = 'JobPageGetAllTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		SELECT
			j.[Id] AS 'JobPage ID',
			j.[URL] AS 'JobPage URL',
			j.[CompanyId] AS 'Company ID'
		FROM [JobPage] j

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
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
DECLARE @TName varchar(20) = 'VacantJobCreateTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		INSERT INTO [VacantJob] ([URL], [CompanyId])
		VALUES
		(@url, @companyId);

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spUpdateVacantJob] (
	@vacantJobId int,
	@companyId int,
	@url varchar(2048))
AS
DECLARE @TName varchar(20) = 'VacantJobUpdateTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		UPDATE [VacantJob]
			SET [URL] = @url,
				[CompanyId] = @companyId
		WHERE [VacantJob].[Id] = @vacantJobId;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spRemoveVacantJob] (
	@vacantJobId int)
AS
DECLARE @TName varchar(20) = 'VacantJobRemoveTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		-- Remove all references to the primary key.
		DELETE FROM [Address] WHERE [JobAdvertVacantJobId] = @vacantJobId;
		DELETE FROM [JobAdvert] WHERE [VacantJobId] = @vacantJobId;
		DELETE FROM [VacantJob] WHERE [Id] = @vacantJobId;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spGetVacantJobById] (
	@vacantJobId int)
AS
DECLARE @TName varchar(20) = 'VacantJobGetByIdTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		SELECT
			v.[Id] AS 'Vacant job ID',
			v.[URL] AS 'VacantJob URL',
			v.[CompanyId] AS 'Company ID'				
		FROM [VacantJob] v
		WHERE [Id] = @vacantJobId;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spGetVacantJobs]
AS
DECLARE @TName varchar(20) = 'VacantJobGetAllTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		SELECT
			v.[Id] AS 'Vacant job ID',
			v.[URL] AS 'VacantJob URL',
			v.[CompanyId] AS 'Company ID'
		FROM [VacantJob] v

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
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
DECLARE @TName varchar(20) = 'CategoryCreateTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		INSERT INTO [Category] ([Name])
		VALUES
		(@categoryName);

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spUpdateCategory] (
	@categoryId int,
	@categoryName varchar(100))
AS
DECLARE @TName varchar(20) = 'CategoryUpdateTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		UPDATE [Category]
			SET [Name] = @categoryName
		WHERE [Category].[Id] = @categoryId;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spRemoveCategory] (
	@categoryId int)
AS
DECLARE @TName varchar(20) = 'CategoryRemoveTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		-- Remove all references to the primary key.
		DELETE FROM [JobAdvert] WHERE [CategoryId] = @categoryId;
		DELETE FROM [DynamicSearchFilter] WHERE [CategoryId] = @categoryId;
		DELETE FROM [Category]	WHERE [Id] = @categoryId;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spGetCategoryById] (
	@categoryId int,
	@categoryName varchar(100))
AS
DECLARE @TName varchar(20) = 'CategoryGetByIdTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		SELECT
			c.[Id] AS 'Category ID',
			c.[Name] AS 'Category Name'
		FROM [Category] c
		WHERE [Id] = @categoryId;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spGetCategories]
AS
DECLARE @TName varchar(20) = 'CategoryGetAllTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		SELECT
			c.[Id] AS 'Category ID',
			c.[Name] AS 'Category Name'
		FROM [Category] c;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spGetCategoryMenu]
AS
DECLARE @TName varchar(20) = 'CategoryGetMenuTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		SELECT
			c.[Id] AS 'Category ID',
			c.[Name] AS 'Category Name',

			s.[Id] AS 'Spec Id',
			s.[Name] AS 'Specialization Name',
			s.[CategoryId] AS 'Specialization Category ID'
		FROM [Category] c 
		LEFT OUTER JOIN [Specialization] s ON s.[CategoryId] = c.[Id];

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
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
DECLARE @TName varchar(20) = 'SpecializationCreateTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		INSERT INTO [Specialization] ([Name], [CategoryId])
		VALUES
		(@specializationName, @categoryId);

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spUpdateSpecialization] (
	@specializationId int,
	@specializationName varchar(100),
	@categoryId int)
AS
DECLARE @TName varchar(20) = 'SpecializationUpdateTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		UPDATE [Specialization]
			SET [Name] = @specializationName,
				[CategoryId] = @categoryId
			WHERE [Id] = @specializationId;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spRemoveSpecialization] (
	@specializationId int)
AS
DECLARE @TName varchar(20) = 'SpecializationRemoveTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY	
		DELETE FROM [JobAdvert] WHERE [SpecializationId] = @specializationId;
		DELETE FROM [DynamicSearchFilter] WHERE [SpecializationId] = @specializationId;
		DELETE FROM [Specialization] WHERE [Id] = @specializationId;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spGetSpecializationById] (
	@specializationId int)
AS
DECLARE @TName varchar(20) = 'SpecializationGetByIdTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		SELECT
			s.[Id] AS 'Specialization ID',
			s.[Name] AS 'Specialization Name',
			s.[CategoryId] AS 'Category ID'
		FROM
			[Specialization] s WHERE s.[Id] = @specializationId;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spGetSpecializations]
AS
DECLARE @TName varchar(20) = 'SpecializationGetAllTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		SELECT
			s.[Id] AS 'Specialization ID',
			s.[Name] AS 'Specialization Name',
			s.[CategoryId] AS 'Category ID'
		FROM
			[Specialization] s;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
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
	@jobAdvertRegistrationDateTime datetime)
AS
DECLARE @TName varchar(20) = 'JobAdvertCreateTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		INSERT INTO [JobAdvert] ([VacantJobId], [CategoryId], [SpecializationId], [Title], [Summary], [RegistrationDateTime])
		VALUES
		(@vacantJobId, @categoryId, @specializationId, @jobAdvertTitle, @jobAdvertSummary, @jobAdvertRegistrationDateTime);

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spUpdateJobAdvert] (
	@vacantJobId int,
    @specializationId int,
	@categoryId int,
	@jobAdvertTitle varchar(100),
	@jobAdvertSummary varchar(250),
	@jobAdvertRegistrationDateTime datetime)
AS
DECLARE @TName varchar(20) = 'JobAdvertUpdateTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		UPDATE [JobAdvert]
			SET	
			[CategoryId] = @categoryId,
			[SpecializationId] = @specializationId,
			[Title] = @jobAdvertTitle,
			[Summary] = @jobAdvertSummary,
			[RegistrationDateTime] = @jobAdvertRegistrationDateTime
			WHERE [JobAdvert].[VacantJobId] = @vacantJobId;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spRemoveJobAdvert] (
	@vacantJobId int)
AS
DECLARE @TName varchar(20) = 'JobAdvertRemoveTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		DELETE FROM [Address] WHERE [JobAdvertVacantJobId] = @vacantJobId;
		DELETE FROM [JobAdvert] WHERE [VacantJobId] = @vacantJobId;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spGetJobAdvertById] (
	@vacantJobId int)
AS
DECLARE @TName varchar(20) = 'JobAdvertGetByIdTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		SELECT
			j.[VacantJobId] AS 'JobAdvert ID',
			j.[CategoryId] AS 'Category ID',
			j.[SpecializationId] AS 'Specialization ID',
			j.[Title] AS 'JobAdvert Title',
			j.[Summary] AS 'JobAdvert Summary',
			j.[RegistrationDateTime] AS 'JobAdvert Registration Date'
		FROM
			[JobAdvert] j WHERE j.[VacantJobId] = @vacantJobId;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spGetJobAdverts]
AS
DECLARE @TName varchar(20) = 'JobAdvertGetAllTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		SELECT
			j.[VacantJobId] AS 'JobAdvert ID',
			j.[CategoryId] AS 'Category ID',
			j.[SpecializationId] AS 'Specialization ID',
			j.[Title] AS 'JobAdvert Title',
			j.[Summary] AS 'JobAdvert Summary',
			j.[RegistrationDateTime] AS 'JobAdvert Registration Date'
		FROM
			[JobAdvert] j;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spGetTotalJobAdvertCountByCategoryId](
	@categoryId int,
	@countByCategory int OUTPUT)
AS
DECLARE @TName varchar(20) = 'JobAdvertCategoryIdCountTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		SET @countByCategory = (SELECT COUNT (*) FROM [JobAdvert] WHERE [CategoryId] = @categoryId);

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spGetTotalJobAdvertCountBySpecializationId](
	@specializationId int,
	@countBySpecialization int OUTPUT)
AS
DECLARE @TName varchar(20) = 'JobAdvertSpecializationIdCountTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		SET @countBySpecialization = (SELECT COUNT (*) FROM [JobAdvert] WHERE [SpecializationId] = @specializationId);

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spGetTotalJobAdvertCountByNonCategorized](
	@countByNonCategorized int OUTPUT)
AS
DECLARE @TName varchar(20) = 'JobAdvertUncategorizedCountTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		SET @countByNonCategorized = (SELECT COUNT (*) FROM [JobAdvert] WHERE [CategoryId] = 0 AND [SpecializationId] = 0);

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
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
DECLARE @TName varchar(20) = 'AddressCreateTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		INSERT INTO [Address] ([JobAdvertVacantJobId], [StreetAddress], [City], [Country], [PostalCode])
		VALUES
		(@jobAdvertVacantJobId, @streetAddress, @city, @country, @postalCode);
		
		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spUpdateAddress](
	@jobAdvertVacantJobId int,
	@streetAddress varchar(250),
	@city varchar(100),
	@country varchar(100),
	@postalCode varchar(10))
AS
DECLARE @TName varchar(20) = 'AddressUpdateTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		UPDATE [Address]
			SET
			[StreetAddress] = @streetAddress,
			[City] = @city,
			[Country] = @country,
			[PostalCode] = @postalCode
		WHERE [JobAdvertVacantJobId] = @jobAdvertVacantJobId;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spRemoveAddress](
	@jobAdvertVacantJobId int)
AS
DECLARE @TName varchar(20) = 'AddressRemoveTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		DELETE FROM [Address] WHERE [JobAdvertVacantJobId] = @jobAdvertVacantJobId;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spGetAddressById](
	@jobAdvertVacantJobId int)
AS
DECLARE @TName varchar(20) = 'AddressGetByIdTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		SELECT 
			a.[JobAdvertVacantJobId] AS 'JobAdvert AddressID',
			a.[StreetAddress] AS 'Street Address', 
			a.[City] AS 'City', 
			a.[Country] AS 'Country', 
			a.[PostalCode] AS 'Postal Code' 
		FROM [Address] a
		WHERE a.[JobAdvertVacantJobId] = @jobAdvertVacantJobId;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spGetAddresses]
AS
DECLARE @TName varchar(20) = 'AddressGetAllTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		SELECT 
			a.[JobAdvertVacantJobId] AS 'JobAdvert AddressID',
			a.[StreetAddress] AS 'Street Address', 
			a.[City] AS 'City', 
			a.[Country] AS 'Country', 
			a.[PostalCode] AS 'Postal Code'
		FROM [Address] a;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
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
DECLARE @TName varchar(20) = 'UserCreateTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		INSERT INTO [User] ([RoleId], [LocationId], [FirstName], [LastName], [Email], [Password], [Salt],[AccessToken])
		VALUES
		(@roleId, @locationId, @userFirstName, @userLastName, @userEmail, @userPass, @userSalt, @userAccessToken);
		
		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
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
DECLARE @TName varchar(20) = 'UserUpdateTransaction';
BEGIN
	BEGIN TRAN @TName;
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
		
		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spRemoveUser](
	@userId int)
AS
DECLARE @TName varchar(20) = 'UserRemoveTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		DELETE FROM [Contract] WHERE [UserId] = @userId;
		DELETE FROM [ConsultantArea] WHERE [UserId] = @userId;
		DELETE FROM [User] WHERE [Id] = @userId;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spGetUserById](
	@userId int)
AS
DECLARE @TName varchar(20) = 'UserGetByIdTransaction';
BEGIN
	BEGIN TRAN @TName;
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
					WHERE c.[UserId] = @userId), 1, 1, '') AS 'Consultant Areas'

		FROM [User] u
			INNER JOIN [Role] r ON r.[Id] = u.[RoleId]
			INNER JOIN [Location] l ON l.[Id] = u.[LocationId]
			INNER JOIN [ConsultantArea] c ON c.[UserId] = u.[Id]
			INNER JOIN [Area] a ON a.[Id] = c.[AreaId]
		WHERE u.[Id] = @userId

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spGetUsers]
AS
DECLARE @TName varchar(20) = 'UserGetAllTransaction';
BEGIN
	BEGIN TRAN @TName;
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
					WHERE c.[UserId] = u.[Id]), 1, 1, '') AS 'Consultant Areas'

		FROM [User] u
			INNER JOIN [Role] r ON r.[Id] = u.[RoleId]
			INNER JOIN [Location] l ON l.[Id] = u.[LocationId]
			INNER JOIN [ConsultantArea] c ON c.[UserId] = u.[Id]
			INNER JOIN [Area] a ON a.[Id] = c.[AreaId];

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spGrantUserArea](
	@userId int,
	@areaId int)
AS
DECLARE @TName varchar(20) = 'UserGrantAreaTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		INSERT INTO [ConsultantArea] ([UserId], [AreaId])
		VALUES (@userId, @areaId);
			
		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spRemoveUserArea](
	@userId int,
	@areaId int)
AS
DECLARE @TName varchar(20) = 'UserRemoveAreaTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
			DELETE FROM [ConsultantArea] WHERE [UserId] = @userId AND [AreaId] = @areaId;
			
			COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spGetUserSaltByEmail](
	@userEmail varchar(50))
AS
DECLARE @TName varchar(20) = 'UserGetSaltByEmailTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		SELECT 
			[Salt] AS 'User Salt'
		FROM [User]
		WHERE [Email] = @userEmail;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spGetUserByAccessToken](
	@userAccessToken varchar(max))
AS
DECLARE @TName varchar(20) = 'UserGetByAccessTokenTransaction';
BEGIN
	BEGIN TRAN @TName;
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

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spUpdateUserSecurity](
	@userId int,
	@userNewPassword varchar(1000),
	@userNewSalt varchar(1000))
AS
DECLARE @TName varchar(20) = 'UserUpdatePasswordTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		UPDATE [User]
			SET
			[Password] = @userNewPassword,
			[Salt] = @userNewSalt
		WHERE [Id] = @userId;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spValidateUserExists](
	@userEmail varchar(50),
	@returnResult bit = 0 OUTPUT)
AS
DECLARE @TName varchar(20) = 'UserValidateExistenceTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		SET @returnResult = 1;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spValidateUserLogin](
	@userEmail varchar(50),
	@userPassword varchar(1000),
	@returnResult bit = 0 OUTPUT)
AS
DECLARE @TName varchar(20) = 'UserValidateLoginTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		SET @returnResult = 1;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

/*##################################################
				## Log Section ##
####################################################*/

DROP PROCEDURE IF EXISTS [JA.spCreateLog]
DROP PROCEDURE IF EXISTS [JA.spUpdateLog]
DROP PROCEDURE IF EXISTS [JA.spRemoveLog]
DROP PROCEDURE IF EXISTS [JA.spGetLogById]
DROP PROCEDURE IF EXISTS [JA.spGetLogs]
GO

CREATE PROCEDURE [JA.spCreateLog] (
	@severityId int,
	@currentTime datetime = GETDATE,
	@createdBy varchar(250),
	@action varchar(250),
	@message varchar(500))
AS
DECLARE @TName varchar(20) = 'LogCreateTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		INSERT INTO [Log] ([LogSeverityId], [CreatedDateTime], [CreatedBy], [Action], [Message]) VALUES
		(@severityId, @currentTime, @createdBy, @action, @message);

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spUpdateLog] (
	@logId int,
	@severityId int,
	@createdDateTime datetime,
	@createdBy varchar(250),
	@action varchar(250),
	@message varchar(500))
AS
DECLARE @TName varchar(20) = 'LogUpdateTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		UPDATE [Log]
			SET
			[LogSeverityId] = @severityId,
			[CreatedDateTime] = @createdDateTime,
			[CreatedBy] = @createdBy,
			[Action] = @action,
			[Message] = @message
			WHERE
			[Id] = @logId;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spRemoveLog] (
	@logId int)
AS
DECLARE @TName varchar(20) = 'LogRemoveTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		DELETE FROM [Log]
		WHERE
		[Log].[Id] = @logId;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spGetLogById] (
	@logId int)
AS
DECLARE @TName varchar(20) = 'LogGetByIdTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		SELECT 
			l.[Id] AS 'Log ID',
			logS.[Severity] AS 'Log Severity',
			l.[CreatedBy],
			l.[CreatedDateTime],
			l.[Action],
			l.[Message]
		FROM [Log] l
		INNER JOIN [LogSeverity] logS ON logS.Id = l.LogSeverityId
		WHERE
			l.[Id] = @logId;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

/*##################################################
			 ## Search Filter Section ##
####################################################*/

DROP PROCEDURE IF EXISTS [JA.spCreateStaticSearchFilter]
DROP PROCEDURE IF EXISTS [JA.spUpdateStaticSearchFilter]
DROP PROCEDURE IF EXISTS [JA.spRemoveStaticSearchFilter]
DROP PROCEDURE IF EXISTS [JA.spGetStaticSearchFilterById]

DROP PROCEDURE IF EXISTS [JA.spCreateDynamicSearchFilter]
DROP PROCEDURE IF EXISTS [JA.spUpdateDynamicSearchFilter]
DROP PROCEDURE IF EXISTS [JA.spRemoveDynamicSearchFilter]
DROP PROCEDURE IF EXISTS [JA.spGetDynamicSearchFilterById]

DROP PROCEDURE IF EXISTS [JA.spGetDynamicFilterKeys]
DROP PROCEDURE IF EXISTS [JA.spGetStaticFilterKeys]
DROP PROCEDURE IF EXISTS [JA.spGetStaticFilterKeysByTypeId]
GO

CREATE PROCEDURE [JA.spCreateStaticSearchFilter] (
	@filterTypeId int,
	@key varchar(50))
AS
DECLARE @TName varchar(20) = 'StatichSearchFilterCreateTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		INSERT INTO [StaticSearchFilter] ([FilterTypeId], [Key]) VALUES
		(@filterTypeId, @key);

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spUpdateStaticSearchFilter] (
	@id int,
	@filterTypeId int,
	@key varchar(50))
AS
DECLARE @TName varchar(20) = 'StaticSearchFilterUpdateTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		UPDATE [StaticSearchFilter]
			SET
			[FilterTypeId] = @filterTypeId,
			[Key] = @key
			WHERE
			[Id] = @id;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spRemoveStaticSearchFilter] (
	@id int)
AS
DECLARE @TName varchar(20) = 'StaticSearchFilterRemoveTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		DELETE FROM [StaticSearchFilter]
		WHERE
		[Id] = @id;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spGetStaticSearchFilterById] (
	@id int)
AS
DECLARE @TName varchar(20) = 'StaticSearchFilterGetByIdTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		SELECT
			s.[Id] AS 'Static Search Filter ID',
			s.[Key],
			f.[Id] AS 'Filter Type ID',
			f.[Name] AS 'Filter Name',
			f.[Description] AS 'Filter Desription'
		FROM [StaticSearchFilter] s
		INNER JOIN [FilterType] f ON f.[Id] = s.[Id]

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spCreateDynamicSearchFilter] (
	@categoryId int,
	@specializationId int,
	@key varchar(50))
AS
DECLARE @TName varchar(20) = 'DynamicSearchFilterCreateTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		INSERT INTO [DynamicSearchFilter] ([CategoryId], [SpecializationId], [Key]) VALUES
		(@categoryId, @specializationId, @key);

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spUpdateDynamicSearchFilter] (
	@id int,
	@categoryId int,
	@specializationId int,
	@key varchar(50))
AS
DECLARE @TName varchar(20) = 'DynamicSearchFilterUpdateTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		UPDATE [DynamicSearchFilter]
			SET
			[CategoryId] = @categoryId,
			[SpecializationId] = @specializationId,
			[Key] = @key
			WHERE
			[Id] = @id;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spRemoveDynamicSearchFilter] (
	@id int)
AS
DECLARE @TName varchar(20) = 'DynamicSearchFilterRemoveTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		DELETE FROM [DynamicSearchFilter]
		WHERE
		[Id] = @id;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spGetDynamicSearchFilterById] (
	@id int)
AS
DECLARE @TName varchar(20) = 'DynamicSearchFilterGetByIdTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		SELECT
			d.[Id] AS 'Dynamic Search Filter ID',
			d.[Key],
			c.[Id] AS 'Category ID',
			c.[Name] AS 'Category Name',
			s.[Id] AS 'Specialization ID',
			s.[CategoryId] AS 'Specialization Category Association ID',
			s.[Name] AS 'Specialization Name'
		FROM [DynamicSearchFilter] d
		INNER JOIN [Category] c ON c.[Id] = d.[CategoryId]
		INNER JOIN [Specialization] s ON s.[Id] = d.[SpecializationId]

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spGetStaticFilterKeys]
AS
DECLARE @TName varchar(20) = 'StaticSearchFilterGetKeysTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		SELECT
			ssf.[Id] AS 'Key ID',
			ft.[Name] AS 'Filter Name',
			ssf.[Key] AS 'Key'
		FROM [StaticSearchFilter] ssf
		INNER JOIN [FilterType] ft ON ft.[Id] = ssf.[FilterTypeId]

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spGetStaticFilterKeysByTypeId](
	@typeId int)
AS
DECLARE @TName varchar(20) = 'StaticSearchFilterGetKeysByTypeIdTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
			SELECT
				ssf.[Id] AS 'Key ID',
				ft.[Name] AS 'Filter Name',
				ssf.[Key] AS 'Key'
			FROM [StaticSearchFilter] ssf
			INNER JOIN [FilterType] ft ON ft.[Id] = @typeId
			WHERE ssf.[FilterTypeId] = @typeId;

			COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spGetDynamicFilterKeys]
AS
DECLARE @TName varchar(20) = 'DynamicSearchFilterGetKeysTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
			SELECT
				dsf.[Id] AS 'Key ID',
				dsf.[Key] AS 'Key',
				dsf.[CategoryId] AS 'Key Category ID',
				dsf.[SpecializationId] AS 'Key Specialization ID'
			FROM [DynamicSearchFilter] dsf;

			COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

/*##################################################
			## Filter Type Section ##
####################################################*/

DROP PROCEDURE IF EXISTS [JA.spCreateFilterType]
DROP PROCEDURE IF EXISTS [JA.spUpdateFilterType]
DROP PROCEDURE IF EXISTS [JA.spRemoveFilterType]
DROP PROCEDURE IF EXISTS [JA.spGetFilterTypeById]
DROP PROCEDURE IF EXISTS [JA.spGetFilterTypes]
GO

CREATE PROCEDURE [JA.spCreateFilterType] (
	@name varchar(50),
	@description varchar(250))
AS
DECLARE @TName varchar(20) = 'FilterTypeCreateTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		INSERT INTO [FilterType] ([Name], [Description]) VALUES
		(@name, @description);

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spUpdateFilterType] (
	@id int,
	@name varchar(50),
	@description varchar(250))
AS
DECLARE @TName varchar(20) = 'FilterTypeUpdateTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		UPDATE [FilterType]
			SET
			[Name] = @name,
			[Description] = @description
			WHERE
			[Id] = @id;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spRemoveFilterType] (
	@id int)
AS
DECLARE @TName varchar(20) = 'FilterTypeRemoveTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		DELETE FROM [StaticSearchFilter] WHERE [FilterTypeId] = @id;

		DELETE FROM [FilterType]
		WHERE [Id] = @id;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spGetFilterTypeById] (
	@id int)
AS
DECLARE @TName varchar(20) = 'FilterTypeGetByIdTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		SELECT
			f.[Id] AS 'Filter Type ID',
			f.[Name] AS 'Filter Name ID',
			f.[Description] AS 'Filter Description'
		FROM [FilterType] f
		WHERE f.[Id] = @id;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spGetFilterTypes]
AS
DECLARE @TName varchar(20) = 'FilterTypeGetAllTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		SELECT
			f.[Id] AS 'Filter Type ID',
			f.[Name] AS 'Filter Name ID',
			f.[Description] AS 'Filter Description'
		FROM [FilterType] f

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

/*##################################################
			## Version Control Section ##
####################################################*/

DROP PROCEDURE IF EXISTS [JA.spCreateVersionControl]
DROP PROCEDURE IF EXISTS [JA.spUpdateVersionControl]
DROP PROCEDURE IF EXISTS [JA.spRemoveVersionControl]
DROP PROCEDURE IF EXISTS [JA.spGetVersionControlById]
DROP PROCEDURE IF EXISTS [JA.spGetVersionControls]

--DROP PROCEDURE IF EXISTS [JA.spGetSystemInformationByName]
GO

CREATE PROCEDURE [JA.spCreateVersionControl] (
	@projectInformationId int,
	@releaseTypeId int,
	@commitId varchar(32),
	@major int,
	@minor int,
	@patch int,
	@releaseDateTime datetime)
AS
DECLARE @TName varchar(20) = 'VersionControlCreateTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		INSERT INTO [VersionControl] ([ProjectInformationId], [ReleaseTypeId], [CommitId], [Major], [Minor], [Patch], [ReleaseDateTime]) VALUES
		(@projectInformationId, @releaseTypeId, @commitId, @major, @minor, @patch, @releaseDateTime);

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spUpdateVersionControl] (
	@id int,
	@projectInformationId int,
	@releaseTypeId int,
	@commitId varchar(32),
	@major int,
	@minor int,
	@patch int,
	@releaseDateTime datetime)
AS
DECLARE @TName varchar(20) = 'VersionControlUpdateTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		UPDATE [VersionControl]
			SET
			[ProjectInformationId] = @projectInformationId,
			[ReleaseTypeId] = @releaseTypeId,
			[CommitId] = @commitId,
			[Major] = @major,
			[Minor] = @minor,
			[Patch] = @patch,
			[ReleaseDateTime] = @releaseDateTime
			WHERE
			[Id] = @id;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spRemoveVersionControl] (
	@id int)
AS
DECLARE @TName varchar(20) = 'VersionControlRemoveTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
			DELETE FROM [VersionControl]
			WHERE [Id] = @id;

			COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spGetVersionControlById] (
	@id int)
AS
DECLARE @TName varchar(20) = 'VersionControlGetByIdTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		SELECT
			v.[Id] AS 'Version ID',
			v.[ProjectInformationId] AS 'Project ID',
			p.[Name] AS 'Project Name',
			p.[PublishedDateTime] AS 'Project Publish Date',
			v.[ReleaseTypeId] AS 'Release Type ID',
			r.[Name] AS 'Release Name',
			v.[CommitId],
			v.[Major],
			v.[Minor],
			v.[Patch],
			v.[ReleaseDateTime] AS 'Version Release Date'
		FROM [VersionControl] v
		INNER JOIN [ProjectInformation] p ON p.[Id] = v.[ProjectInformationId]
		INNER JOIN [ReleaseType] r ON r.[Id] = v.[ReleaseTypeId]
		WHERE v.[Id] = @id;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spGetVersionControls]
AS
DECLARE @TName varchar(20) = 'VersionControlGetAllTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		SELECT
			v.[Id] AS 'Version ID',
			v.[ProjectInformationId] AS 'Project ID',
			p.[Name] AS 'Project Name',
			p.[PublishedDateTime] AS 'Project Publish Date',
			v.[ReleaseTypeId] AS 'Release Type ID',
			r.[Name] AS 'Release Name',
			v.[CommitId],
			v.[Major],
			v.[Minor],
			v.[Patch],
			v.[ReleaseDateTime] AS 'Version Release Date'
		FROM [VersionControl] v
		INNER JOIN [ProjectInformation] p ON p.[Id] = v.[ProjectInformationId]
		INNER JOIN [ReleaseType] r ON r.[Id] = v.[ReleaseTypeId]

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

--CREATE PROCEDURE [JA.spGetSystemInformationByName] (
--	@systemName varchar(50))
--AS
--	DECLARE @transaction varchar(20)
--	SET @transaction = 'Getting Information for System ' + @systemName;

--	BEGIN TRANSACTION @transaction WITH MARK N'Acquiring System Information';

--	BEGIN TRY

--		BEGIN
--			SELECT TOP (1)
--				v.[HashId] as 'Commit ID',
--				s.[Name] as 'System Name',
--				CONCAT('v',
--					CONCAT_WS (
--						'.'
--						,v.[Major], v.[Minor], v.[Patch]
--				), '-', r.[Name]) AS 'Version',
--				s.[PublishedDateTime] as 'Published Date',
--				v.[ReleaseDateTime] as 'Released Date'
--			FROM [Version] v

--			INNER JOIN [System] s ON s.[Name] = @systemName
--			INNER JOIN [ReleaseType] r ON r.[Id] = v.[ReleaseTypeId]
--			WHERE v.[SystemName] = @systemName
--			ORDER BY v.[ReleaseDateTime] DESC;
--			COMMIT TRANSACTION @transaction;
--		END

--	END TRY
--	BEGIN CATCH
		
--		BEGIN
--			ROLLBACK TRANSACTION @transaction;
--		END

--	END CATCH
--GO

/*##################################################
		 ## Project Information Section ##
####################################################*/

DROP PROCEDURE IF EXISTS [JA.spCreateProjectInformation]
DROP PROCEDURE IF EXISTS [JA.spUpdateProjectInformation]
DROP PROCEDURE IF EXISTS [JA.spRemoveProjectInformation]
DROP PROCEDURE IF EXISTS [JA.spGetProjectInformationById]
DROP PROCEDURE IF EXISTS [JA.spGetProjectInformations]
GO

CREATE PROCEDURE [JA.spCreateProjectInformation] (
	@name varchar(50),
	@publishedDateTime datetime)
AS
DECLARE @TName varchar(20) = 'ProjectInformationCreateTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		INSERT INTO [ProjectInformation] ([Name], [PublishedDateTime]) VALUES
		(@name, @publishedDateTime);

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spUpdateProjectInformation] (
	@id int,
	@name varchar(50),
	@publishedDateTime datetime)
AS
DECLARE @TName varchar(20) = 'ProjectInformationUpdateTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		UPDATE [ProjectInformation]
			SET
			[Name] = @name,
			[PublishedDateTime] = @publishedDateTime
			WHERE
			[Id] = @id;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spRemoveProjectInformation] (
	@id int)
AS
DECLARE @TName varchar(20) = 'ProjectInformationRemoveTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		DELETE FROM [ProjectInformation]
		WHERE [Id] = @id;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spGetProjectInformationById] (
	@id int)
AS
DECLARE @TName varchar(20) = 'ProjectInformationGetByIdTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		SELECT
			p.[Id] AS 'Project ID',
			p.[Name] AS 'Project Name',
			p.[PublishedDateTime] AS 'Project Published'
		FROM [ProjectInformation] p
		WHERE [Id] = @id;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spGetProjectInformations]
AS
DECLARE @TName varchar(20) = 'ProjectInformationGetAllTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		SELECT
			p.[Id] AS 'Project ID',
			p.[Name] AS 'Project Name',
			p.[PublishedDateTime] AS 'Project Published'
		FROM [ProjectInformation] p

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

/*##################################################
		 ## Release Type Section ##
####################################################*/

DROP PROCEDURE IF EXISTS [JA.spCreateReleaseType]
DROP PROCEDURE IF EXISTS [JA.spUpdateReleaseType]
DROP PROCEDURE IF EXISTS [JA.spRemoveReleaseType]
DROP PROCEDURE IF EXISTS [JA.spGetReleaseTypeById]
DROP PROCEDURE IF EXISTS [JA.spGetReleaseTypes]
GO

CREATE PROCEDURE [JA.spCreateReleaseType] (
	@name varchar(100))
AS
DECLARE @TName varchar(20) = 'ReleaseTypeCreateTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		INSERT INTO [ReleaseType] ([Name]) VALUES
		(@name);

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spUpdateReleaseType] (
	@id int,
	@name varchar(100))
AS
DECLARE @TName varchar(20) = 'ReleaeTypeUpdateTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		UPDATE [ReleaseType]
			SET
			[Name] = @name
			WHERE
			[Id] = @id;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spRemoveReleaseType] (
	@id int)
AS
DECLARE @TName varchar(20) = 'ReleaseTypeRemoveTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		DELETE FROM [ReleaseType]
		WHERE [Id] = @id;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spGetReleaseTypeById] (
	@id int)
AS
DECLARE @TName varchar(20) = 'ReleaseTypeGetByIdTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		SELECT
			p.[Id] AS 'Release ID',
			p.[Name] AS 'Release Name'
		FROM [ReleaseType] p
		WHERE [Id] = @id;

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO

CREATE PROCEDURE [JA.spGetReleaseTypes]
AS
DECLARE @TName varchar(20) = 'ReleaseTypeGetAllTransaction';
BEGIN
	BEGIN TRAN @TName;
	BEGIN TRY
		SELECT
			p.[Id] AS 'Release ID',
			p.[Name] AS 'Release Name'
		FROM [ReleaseType] p

		COMMIT TRANSACTION @TName;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRAN @TName;
		END 
	END CATCH
END;
GO