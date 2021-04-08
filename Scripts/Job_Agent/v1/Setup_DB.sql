USE [master]
GO

DECLARE @kill varchar(8000) = ''
SELECT @kill = @kill + 'kill ' + CONVERT(varchar(5), session_id) + ';'
FROM sys.dm_exec_sessions
WHERE database_id = DB_ID('JobAgentDB')

EXEC(@kill)
GO

DROP DATABASE IF EXISTS [JobAgentDB]
GO

CREATE DATABASE [JobAgentDB]
GO

USE [JobAgentDB]
GO

DROP TABLE IF EXISTS [ConsultantArea]
CREATE TABLE [ConsultantArea] (
-- Column Properties
[Id] int not null identity(1,1),
[Name] varchar(100) not null

-- Define Column Primary Key
PRIMARY KEY ([Id])
)
GO

DROP TABLE IF EXISTS [Location]
CREATE TABLE [Location] (
-- Column Properties
[Id] int not null identity(1,1),
[Name] varchar(100) not null,
[Description] varchar(150),

-- Define Column Primary Key
PRIMARY KEY ([Id])
)
GO

DROP TABLE IF EXISTS [Category]
CREATE TABLE [Category] (
-- Column Properites
[Id] int identity,
[Name] varchar(150)

-- Define Column Primary Key
PRIMARY KEY ([Id])
)
GO

DROP TABLE IF EXISTS [Specialization]
CREATE TABLE [Specialization] (
-- Column Properties
[Id] int identity,
[Name] varchar(100),
[CategoryId] int,

-- Define Column Primary Key
PRIMARY KEY ([Id]),

-- Define References to Other Keys
FOREIGN KEY ([CategoryId]) REFERENCES [Category]([Id])
)
GO

DROP TABLE IF EXISTS [User]
CREATE TABLE [User] (
-- Column Properties
[Id] int not null identity(1,1),
[FirstName] varchar(128) not null,
[LastName] varchar(128) not null,
[Email] varchar(255) not null,
[Password] varchar(255) not null,
[Salt] varchar(128) not null,
[AccessToken] varchar(8000),
[ConsultantAreaId] int not null,
[LocationId] int not null

-- Define Column Primary Key
PRIMARY KEY ([Id]),

-- Define References to Others Keys
FOREIGN KEY ([ConsultantAreaId]) REFERENCES [ConsultantArea]([Id]),
FOREIGN KEY ([LocationId]) REFERENCES [Location]([Id])
)
GO

DROP TABLE IF EXISTS [Company]
CREATE TABLE [Company] (
-- Column Properties
[Id] int not null identity(1,1),
[CVR] int not null,
[Name] varchar(128) not null,
[URL] varchar(1000) not null,

-- Define Column Primary Key
PRIMARY KEY ([Id])
)
GO

DROP TABLE IF EXISTS [Contract]
CREATE TABLE [Contract] (
-- Column Properites
[Id] int not null identity(1,1),
[ContactPerson] varchar(255) not null,
[ContractName] varchar(255) not null,
[ExpiryDate] Date not null DEFAULT CURRENT_TIMESTAMP,
[RegisteredDate] Date not null DEFAULT CURRENT_TIMESTAMP,
[SignedByUserId] int not null,
[CompanyId] int not null,

-- Define Column Primary Key
PRIMARY KEY ([Id]),

-- Define References to Other Keys
FOREIGN KEY ([SignedByUserId]) REFERENCES [User]([Id]),
FOREIGN KEY ([CompanyId]) REFERENCES [Company]([Id])
)
GO

DROP TABLE IF EXISTS [JobAdvert]
CREATE TABLE [JobAdvert] (
-- Column Properties
[Id] int not null identity(1,1),
[Title] varchar(255) not null,
[Email] varchar(128),
[PhoneNumber] varchar(64),
[JobDescription] varchar(MAX) not null,
[JobLocation] varchar(255) not null,
[JobRegisteredDate] Date not null DEFAULT CURRENT_TIMESTAMP,
[DeadlineDate] Date not null,
[SourceURL] varchar(1000) not null,
[CompanyId] int not null,
[CategoryId] int not null,
[SpecializationId] int default 0,

-- Define Column Primary Key
PRIMARY KEY ([Id]),

-- Define References to Other Keys
FOREIGN KEY ([CompanyId]) REFERENCES [Company]([Id]),
FOREIGN KEY ([CategoryId]) REFERENCES [Category]([Id]),
FOREIGN KEY ([SpecializationId]) REFERENCES [Specialization]([Id])
)
GO

DROP TABLE IF EXISTS [SourceLink]
CREATE TABLE [SourceLink] (
[Id] int identity not null,
[CompanyId] int not null,
[Link] varchar(500) not null,

-- Set Primary Key
PRIMARY KEY ([Id]),

-- Get Foreign Key
FOREIGN KEY ([CompanyId]) REFERENCES [Company]([Id])
)
GO