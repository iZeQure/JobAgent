-- USE Database to inject procedures into.
USE [JobAgentDB]
GO

/*
	## Contract ##
*/

-- Drop Contract Procedures if exists.
DROP PROCEDURE IF EXISTS [CreateContract]
DROP PROCEDURE IF EXISTS [UpdateContract]
DROP PROCEDURE IF EXISTS [RemoveContract]
DROP PROCEDURE IF EXISTS [GetContractById]
DROP PROCEDURE IF EXISTS [GetAllContracts]


GO

-- Create a contract.
CREATE PROCEDURE [CreateContract]
	@contactPerson varchar(255),
	@contractName varchar(255),
	@expiryDate date,
	@registeredDate date,
	@signedByUserId int,
	@companyId int
AS
	-- Check if a Company already owns a contract.
	IF NOT EXISTS (SELECT * FROM [Contract] WHERE [Contract].[CompanyId] = @companyId)
		-- Check if the user exists in the database who signs the contract.
		IF EXISTS (SELECT * FROM [User] WHERE [User].[Id] = @SignedByUserId)
			-- Create Contract
			INSERT INTO [Contract] ([ContactPerson], [ContractName], [ExpiryDate], [RegisteredDate], [SignedByUserId], [CompanyId])
			VALUES
			(
				@contactPerson, @contractName, (SELECT DATEADD(YEAR, 5, @registeredDate) AS ExpiryDate), @registeredDate, @signedByUserId, @companyId
			)
GO

-- Update contract
CREATE PROCEDURE [UpdateContract]
	@id int,
	@companyId int,
	@signedByUserId int,
	@contactPerson varchar(255),
	@regDate date,
	@expiryDate date
AS
	-- Check if the contract exists.
	IF EXISTS (SELECT * FROM [Contract] WHERE [Contract].[Id] = @id)
		-- Update Fields for contract
		UPDATE [Contract]
		SET
			[CompanyId] = @companyId,
			[SignedByUserId] = @signedByUserId,
			[ContactPerson] = @contactPerson,
			[RegisteredDate] = @regDate,
			[ExpiryDate] = @expiryDate
		WHERE
			[Contract].[Id] = @id
GO

-- Remove contract by id.
CREATE PROCEDURE [RemoveContract]
	@id int
AS
	-- Check if the contract exists
	IF EXISTS (SELECT * FROM [Contract] WHERE [Contract].[Id] = @id)

		DELETE FROM [SourceLink]
		WHERE
			[SourceLink].[CompanyId] = @id

		-- Delete Contract
		DELETE FROM [Contract]
		WHERE
			[Contract].[Id] = @id
GO

-- Get contract by id.
CREATE PROCEDURE [GetContractById]
	-- Contract Id
	@id int
AS
	-- Check if the contract exists by id
	IF EXISTS (SELECT * FROM [Contract] WHERE [Contract].[Id] = @id)
		SELECT
			-- Contract Information
			[Contract].[Id],
			[Contract].[ContactPerson],
			[Contract].[ContractName],
			[Contract].[ExpiryDate],
			[Contract].[RegisteredDate],
			-- User Information
			[User].[Id] 'UserId',
			[User].[FirstName],
			[User].[LastName],
			-- Company Information
			[Company].[Id] AS 'CompanyId',
			[Company].[CVR],
			[Company].[Name]
		FROM
			[Contract]
		LEFT JOIN [User]
			ON	[Contract].[SignedByUserId] = [User].[Id]
		LEFT JOIN [Company]
			ON [Contract].[CompanyId] = [Company].[Id]
		WHERE 
			[Contract].[Id] = @id
GO

-- Get all contracts.
CREATE PROCEDURE [GetAllContracts]
AS
	SELECT
		-- Contract Information
		[Contract].[Id],
		[Contract].[ContactPerson],
		[Contract].[ContractName],
		[Contract].[ExpiryDate],
		[Contract].[RegisteredDate],
		-- User Information
		[User].[Id] 'UserId',
		[User].[FirstName],
		[User].[LastName],
		-- Company Information
		[Company].[Id] AS 'CompanyId',
		[Company].[CVR],
		[Company].[Name]
	FROM
		[Contract]
	LEFT JOIN [User]
		ON	[Contract].[SignedByUserId] = [User].[Id]
	LEFT JOIN [Company]
		ON [Contract].[CompanyId] = [Company].[Id]
GO