USE [master]
GO

DECLARE @kill varchar(8000) = ''
SELECT @kill = @kill + 'kill ' + CONVERT(varchar(5), session_id) + ';'
FROM sys.dm_exec_sessions
WHERE database_id = DB_ID('JobAgentDB_v2')

EXEC(@kill)
GO

DROP DATABASE IF EXISTS [JobAgentDB_v2]
GO

CREATE DATABASE [JobAgentDB_v2]
GO

USE [JobAgentDB_v2]
GO

/*##################################################
				## Drop Tables ##
####################################################*/

DROP TABLE IF EXISTS [Category]
DROP TABLE IF EXISTS [Specialization]
DROP TABLE IF EXISTS [Area]
DROP TABLE IF EXISTS [Role]
DROP TABLE IF EXISTS [Location]
DROP TABLE IF EXISTS [VacantJob]
DROP TABLE IF EXISTS [JobAdvert]
DROP TABLE IF EXISTS [Address]
DROP TABLE IF EXISTS [ConsultantArea]
DROP TABLE IF EXISTS [User]
DROP TABLE IF EXISTS [Contract]
DROP TABLE IF EXISTS [JobPage]
DROP TABLE IF EXISTS [Company]

DROP TABLE IF EXISTS [Version]
DROP TABLE IF EXISTS [System]
DROP TABLE IF EXISTS [ReleaseType]
DROP TABLE IF EXISTS [StaticSearchFilter]
DROP TABLE IF EXISTS [DynamicSearchFilter]
DROP TABLE IF EXISTS [FilterType]

DROP TABLE IF EXISTS [Log]
DROP TABLE IF EXISTS [LogSeverity]
GO

/*##################################################
				## Setup ##
####################################################*/

CREATE TABLE [JobAdvert] (
[VacantJobId] int not null,
[CategoryId] int not null,
[SpecializationId] int not null,
[Title] varchar(100) default 'Oplysninger mangler',
[Summary] varchar(250) default 'Oplysninger mangler',
[RegistrationDateTime] datetime default GETDATE())
GO

CREATE TABLE [Address](
[JobAdvertVacantJobId] int not null,
[StreetAddress] varchar(250) not null,
[City] varchar(100) not null,
[Country] varchar(100) not null,
[PostalCode] varchar(10) not null)
GO


CREATE TABLE [VacantJob] (
[Id] int not null identity(1,1),
[CompanyId] int not null,
[URL] varchar(2048) not null)
GO

CREATE TABLE [Company] (
[Id] int not null identity(1,1),
[CVR] int not null,
[Name] varchar(50) not null,
[ContactPerson] varchar(50) not null)
GO

CREATE TABLE [JobPage](
[Id] int not null identity(1,1),
[CompanyId] int not null,
[URL] varchar(2048))
GO

CREATE TABLE [Contract] (
[CompanyId] int not null,
[UserId] int not null,
[Name] uniqueidentifier not null,
[RegistrationDateTime] datetime default GETDATE(),
[ExpiryDateTime] datetime default DATEADD(YEAR, 5, GETDATE()))
GO

CREATE TABLE [User] (
[Id] int not null identity(1,1),
[RoleId] int not null,
[LocationId] int not null,
[FirstName] varchar(50) not null,
[LastName] varchar(50) not null,
[Email] varchar(50) not null unique,
[Password] varchar(1000) not null,
[Salt] varchar(1000) not null,
[AccessToken] varchar(max) not null)
GO

CREATE TABLE [ConsultantArea] (
[UserId] int not null,
[AreaId] int not null)
GO

CREATE TABLE [Role] (
[Id] int not null identity(1,1),
[Name] varchar(30) not null,
[Description] varchar(100) default 'Ingen beskrivelse på denne rolle.')
GO

CREATE TABLE [Location] (
[Id] int not null identity(1,1),
[Name] varchar(30) not null)
GO

CREATE TABLE [Area] (
[Id] int not null identity(1,1),
[Name] varchar(50) not null)
GO

CREATE TABLE [Category] (
[Id] int not null identity(1,1),
[Name] varchar(100) not null)
GO

CREATE TABLE [Specialization] (
[Id] int not null identity(1,1),
[CategoryId] int not null,
[Name] varchar(100) not null)
GO

CREATE TABLE [System] (
[Name] varchar(50) not null,
[PublishedDateTime] datetime not null default GETDATE())
GO

CREATE TABLE [ReleaseType] (
[Id] int not null identity(1,1),
[Name] varchar(100) not null)
GO

CREATE TABLE [Version] (
[HashId] varchar(40) not null,
[SystemName] varchar(50) not null,
[ReleaseTypeId] int not null,
[Major] int not null default 0,
[Minor] int not null default 0,
[Patch] int not null default 0,
[ReleaseDateTime] datetime default GETDATE())
GO

CREATE TABLE [FilterType] (
[Id] int not null identity(1,1),
[Name] varchar(50) not null,
[Description] varchar(250) default 'None')
GO

CREATE TABLE [StaticSearchFilter] (
[Id] int not null identity(1,1),
[FilterTypeId] int not null,
[Key] varchar(50) not null)
GO

CREATE TABLE [DynamicSearchFilter] (
[Id] int not null identity(1,1),
[CategoryId] int not null,
[SpecializationId] int not null,
[Key] varchar(50) not null)
GO

CREATE TABLE [LogSeverity] (
[Id] int not null identity(1,1),
[Severity] varchar(50))
GO

CREATE TABLE [Log] (
[Id] int not null identity(1,1),
[LogSeverityId] int not null,
[CreatedDateTime] datetime not null default GETDATE(),
[CreatedBy] varchar(250) not null,
[Action] varchar(250) not null,
[Message] varchar(500) not null)
GO

/*##################################################
				## Alter Data Tables ##
####################################################*/

ALTER TABLE [Area]
ADD 
	PRIMARY KEY ([Id])
GO

ALTER TABLE [Location]
ADD
	PRIMARY KEY ([Id])
GO

ALTER TABLE [Role]
ADD
	PRIMARY KEY ([Id])
GO

ALTER TABLE [User]
ADD
	PRIMARY KEY ([Id]),
	FOREIGN KEY ([RoleId]) REFERENCES [Role] ([Id])	ON UPDATE CASCADE,
	FOREIGN KEY ([LocationId]) REFERENCES [Location] ([Id])	ON UPDATE CASCADE
GO

ALTER TABLE [ConsultantArea]
ADD
	PRIMARY KEY ([UserId], [AreaId]),
	FOREIGN KEY ([UserId]) REFERENCES [User] ([Id])	ON UPDATE CASCADE,
	FOREIGN KEY ([AreaId]) REFERENCES [Area] ([Id])	ON UPDATE CASCADE
GO

ALTER TABLE [Company]
ADD
	PRIMARY KEY ([Id])
GO

ALTER TABLE [JobPage]
ADD
	PRIMARY KEY ([Id]),
	FOREIGN KEY ([CompanyId]) REFERENCES [Company] ([Id]) ON DELETE CASCADE
GO

ALTER TABLE [Contract]
ADD
	PRIMARY KEY ([CompanyId]),
	FOREIGN KEY ([CompanyId]) REFERENCES [Company] ([Id]) ON UPDATE CASCADE,
	FOREIGN KEY ([UserId]) REFERENCES [User] ([Id])	ON UPDATE CASCADE
GO

ALTER TABLE [VacantJob]
ADD
	PRIMARY KEY ([Id]),
	FOREIGN KEY ([CompanyId]) REFERENCES [Company] ([Id]) ON UPDATE CASCADE
GO

ALTER TABLE [Category]
ADD
	PRIMARY KEY ([Id])
GO

ALTER TABLE [Specialization]
ADD
	PRIMARY KEY ([Id]),
	FOREIGN KEY ([CategoryId]) REFERENCES [Category] ([Id])	ON UPDATE CASCADE
GO

ALTER TABLE [JobAdvert]
ADD
	PRIMARY KEY ([VacantJobId]),
	FOREIGN KEY ([VacantJobId]) REFERENCES [VacantJob] ([Id]),
	FOREIGN KEY ([CategoryId]) REFERENCES [Category] ([Id]),
	FOREIGN KEY ([SpecializationId]) REFERENCES [Specialization] ([Id])
GO

ALTER TABLE[Address]
ADD
	PRIMARY KEY ([JobAdvertVacantJobId]),
	FOREIGN KEY ([JobAdvertVacantJobId]) REFERENCES [JobAdvert] ([VacantJobId])
GO

ALTER TABLE [System]
ADD
	PRIMARY KEY ([Name])
GO

ALTER TABLE [ReleaseType]
ADD
	PRIMARY KEY ([Id])
GO

ALTER TABLE [FilterType]
ADD
	PRIMARY KEY ([Id])
GO

ALTER TABLE [Version]
ADD 
	PRIMARY KEY ([HashId], [SystemName]),
	FOREIGN KEY ([SystemName]) REFERENCES [System] ([Name]),
	FOREIGN KEY ([ReleaseTypeId]) REFERENCES [ReleaseType] ([Id])
GO

ALTER TABLE [StaticSearchFilter]
ADD
	PRIMARY KEY ([Id]),
	FOREIGN KEY ([FilterTypeId]) REFERENCES [FilterType] ([Id])
GO

ALTER TABLE [DynamicSearchFilter]
ADD
	PRIMARY KEY ([Id]),
	FOREIGN KEY ([CategoryId]) REFERENCES [Category] ([Id]),
	FOREIGN KEY ([SpecializationId]) REFERENCES [Specialization] ([Id])
GO

ALTER TABLE [LogSeverity]
ADD
	PRIMARY KEY ([Id])
GO

ALTER TABLE [Log]
ADD
	PRIMARY KEY ([Id]),
	FOREIGN KEY ([LogSeverityId]) REFERENCES [LogSeverity] ([Id])
GO

/*##################################################
		  ## Insert Dummy / Static Data ##
####################################################*/

INSERT INTO [Area] ([Name])
VALUES
('Personvognsmekaniker'),
('Lastvognsmekaniker'),
('Vejgodstransportuddannelsen'),
('Lager- og terminaluddannelsen'),
('Entreprenør og landbrugsmaskin udd.'),
('Kontoruddannelse med specialer'),
('Handelsuddannelse med specialer'),
('Finansuddannelsen'),
('Eventkoordinator'),
('Detailhandelsuddannelsen med specialer'),
('Skolepraktik'),
('Murer'),
('Træfagenes byggeuddannelse'),
('Byggemontage teknikker'),
('VVS-energi'),
('Bygningsmaler'),
('Bager og konditor'),
('Ernæringsassistent'),
('Gourmetslagter/Industrislagter'),
('Gastronom'),
('Tjener'),
('Receptionist'),
('Data- og kommunikationsuddannelsen'),
('Elektronik- og svagstrømsuddannelsen'),
('Elektriker'),
('Industriteknikeruddannelsen'),
('Værktøjsmager'),
('Procesoperatør'),
('Smed'),
('Landbrugsuddannelsen'),
('Anlægsgartner'),
('Anlægsstruktør'),
('Serviceassistent'),
('Sikkerhedsvagt'),
('Gourmetslagter'),
('Slagter'),
('Tarmrenser'),
('Styrket praktik i Regionen'),
('Fødevare'),
('Merkantil'),
('Industri'),
('Landbrug / Gartner'),
('Transport'),
('Byggeri'),
('SOSU'),
('Ernæringsassistener'),
('Stem'),
('AUB Merkantil')
GO

INSERT INTO [Location] ([Name])
VALUES
('ZBC Holbæk'),
('ZBC Køge'),
('ZBC Næstved'),
('ZBC Ringsted'),
('ZBC Roskilde'),
('ZBC Slagelse'),
('ZBC Vordingborg'),
('ZBC Kalundborg')
GO

INSERT INTO [Role] ([Name], [Description])
VALUES
('Konsulent', 'Giver adgang til basale handlinger og funktioner.'),
('Administrator', 'Giver adgang til administrative værktøjer.'),
('Udvikler', 'Fuld system adgang.')
GO

INSERT INTO [Category] ([Name])
VALUES
('Anlægsgartner'),
('Anlægsstruktør, bygningstruktør og brolægger'),
('Autolakerer'),
('Automatik- og procesuddannelsen'),
('Bager og konditoruddannelsen'),
('Beklædningshåndværker'),
('Beslagsmed'),
('Boligmonteringsuddannelsen'),
('Buschauffør i kollektiv trafik'),
('Byggemontagetekniker'),
('Bygningsmaler'),
('Bygningssnedker'),
('Bådmekaniker'),
('Cnc-teknikeruddannelsen'),
('Cykel- og motorcykelmekaniker'),
('Data- og kommunikationsuddannelsen'),
('Den pædagogiske assistentuddannelse'),
('Detailhandel'),
('Digital media'),
('Dyrepasser'),
('Ejendomsservicetekniker'),
('Elektriker'),
('Elektronik- og svagstrømsuddannelsen'),
('Elektronikoperatør'),
('Entreprenør- og landbrugsmaskinuddannelsen'),
('Ernæringsassistent'),
('Eventkoordinator'),
('Film- og tv-produktionsuddannelsen'),
('Finansuddannelsen'),
('Finmekanikeruddannelsen'),
('Fitnessuddannelse'),
('Flytekniker'),
('Forsyningsoperatør'),
('Fotograf'),
('Frisør'),
('Gartner'),
('Gastronom'),
('Glarmester'),
('Gourmetslagter'),
('Grafisk tekniker'),
('Greenkeeper'),
('Guld- og sølvsmedeuddannelsen'),
('Handelsuddannelsen'),
('Havne- og terminaluddannelsen'),
('Hospitalsteknisk assistent'),
('Industrioperatør'),
('Industrislagter'),
('Industriteknikeruddannelsen'),
('Karrosseriteknikeruddannelsen'),
('Kontoruddannelsen'),
('Kosmetiker'),
('Kranfører'),
('Køletekniker'),
('Lager- og terminaluddannelsen'),
('Landbrugsuddannelsen'),
('Lastvognsmekaniker'),
('Lufthavnsuddannelsen'),
('Maritime håndværksfag'),
('Maskinsnedker'),
('Mediegrafiker'),
('Mejerist'),
('Murer'),
('Møbelsnedker og orgelbygger'),
('Ortopædist'),
('Overfladebehandler'),
('Personvognsmekaniker'),
('Plastmager'),
('Procesoperatør'),
('Produktions- og montageuddannelsen'),
('Produktør'),
('Receptionist'),
('Redderuddannelsen'),
('Serviceassistent'),
('Sikkerhedsvagt'),
('Skibsmekaniker'),
('Skibsmontør'),
('Skiltetekniker'),
('Skorstensfejer'),
('Skov- og naturtekniker'),
('Smed'),
('Social- og sundhedsuddannelsen'),
('Stenhugger'),
('Stukkatør'),
('Støberitekniker'),
('Tagdækker'),
('Tandklinikassistent'),
('Tandtekniker'),
('Tarmrenser'),
('Teater-, udstillings- og eventtekniker'),
('Teknisk designer'),
('Teknisk isolatør'),
('Tjener'),
('Togklargøringsuddannelsen'),
('Træfagenes byggeuddannelse'),
('Turistbuschauffør'),
('Urmager'),
('Vejgodstransportuddannelsen'),
('Veterinærsygeplejerske'),
('VVS-energiuddannelsen'),
('Værktøjsuddannelsen'),
('Webudvikler'),
('Lokomotivfører'),
('Fodterapeut')
GO

INSERT INTO [Specialization] ([Name], [CategoryId])
VALUES
('Automatikmontør', 4),
('Automatiktekniker', 4),
('Automatiseringstekniker', 4),
('Detailbager, håndværksbager eller konditor', 5),
('Bagværker', 5),
('Beklædningshåndværker', 6),
('Tekstil- og beklædningsassistent', 6),
('Buschauffør', 9),
('Buschauffør i kollektiv trafik', 9),
('Kørselsdisponent', 9),
('Motorcykelmekaniker', 15),
('Cykelmekaniker', 15),
('Knallertmekaniker', 15),
('Infrastruktur', 16),
('Programmering', 16),
('It-supporter', 16),
('Dyrepasser', 20),
('Ejendomsservicetekniker', 21),
('Elektronik udviklingstekniker', 23),
('Medicotekniker', 23),
('Radio-tv-fagtekniker', 23),
('Elektronikfagtekniker', 23),
('Entreprenør- og landbrugsmaskinuddannelsen', 25),
('Materielmekaniker', 25),
('Film- og tv-produktionsuddannelsen', 28),
('Film- og tv-assistent', 28),
('Finmekaniker', 30),
('Finmekanikerassistent', 30),
('Gastronom', 37),
('Gastronomassistent', 37),
('Gourmetslagter', 39),
('Gourmetslagteraspirant', 39),
('Greenkeeper', 41),
('Groundsman', 41),
('Guld- og sølvsmed', 42),
('Butiksguldsmed', 42),
('Havne- og terminalarbejder', 44),
('Havnemedhjælper', 44),
('Industrioperatør', 46),
('Industrioperatør-produktivitet', 46),
('Slagter m. speciale', 47),
('Slagter (trin 1)', 47),
('Industritekniker produktion 5', 48),
('Industritekniker maskin 4', 48),
('Industriassistent 3', 48),
('Kranfører', 52),
('Kørseldisponent', 52),
('Køletekniker', 53),
('Køleassistent', 53),
('Lager- og terminaldisponent', 54),
('Lageroperatør – lager- og transportoperatør', 54),
('Lageroperatør – lager- og logistikoperatør', 54),
('Lagermedhjælper', 54),
('Landbrugsassistent', 55),
('Landbrugsuddannelsen', 55),
('Produktionsleder', 55),
('Agrarøkonom', 55),
('Lastvognsmekaniker', 56),
('Lastvognsmontør', 56),
('Maskinsnedker', 59),
('Produktionsassistent, døre og vinduer', 59),
('Produktionsassistent, møbel', 59),
('Mejerist', 61),
('Mejerioperatør', 61),
('Møbelsnedker', 63),
('Orgelbygger', 63),
('Overfladebehandler m. speciale', 65),
('Overfladebehandler', 65),
('Personvognsmekaniker', 66),
('Personvognsmontør', 66),
('Procesoperatør', 68),
('Procesoperatør med speciale i pharma', 68),
('Procesarbejder', 68),
('Skibsmekaniker', 75),
('Skibsmontør', 76),
('Industrimontør', 76),
('Stentekniker', 82),
('Stenhugger', 82),
('Teater-, event- og av-teknisk produktionsassistent', 89),
('Teater-, event- og av-tekniker', 89),
('Teknisk designer', 90),
('Tjener', 92),
('Konference- og selskabstjener', 92),
('Klargøring og rangering af persontog', 93),
('Klargøring og rangering af godstog', 93),
('Turistbuschauffør', 95),
('Kørselsdisponent', 95),
('Godschauffør', 97),
('Renovationschauffør', 97),
('Tankbilchauffør', 97),
('Flyttechauffør', 97),
('Kørseldisponent', 97),
('Værktøjsuddannelsen', 100),
('Værktøjsmager', 100)
GO

SET IDENTITY_INSERT [dbo].[Category] ON;
INSERT INTO [Category] ([Id], [Name])
VALUES
(0, 'Ingen Kategori')
GO
SET IDENTITY_INSERT [dbo].[Category] OFF;
GO

SET IDENTITY_INSERT [dbo].[Specialization] ON;
INSERT INTO [Specialization] ([Id], [Name], [CategoryId])
VALUES
(0, 'Intet Speciale', 0)
GO
SET IDENTITY_INSERT [dbo].[Specialization] OFF;
GO

INSERT INTO [System] ([Name], [PublishedDateTime]) VALUES 
('Din Job Agent', GETDATE()),
('Zombie', GETDATE())
GO

INSERT INTO [ReleaseType] ([Name]) VALUES
('stable'),
('test'),
('dev'),
('alpha'),
('beta')
GO

INSERT INTO [FilterType] ([Name], [Description]) VALUES
('title', NULL),
('summary', NULL),
('description', NULL),
('email', NULL),
('phone_number', NULL),
('registration_datetime', NULL),
('application_deadline_datetime', NULL),
('category', NULL),
('specialization', NULL),
('street_address', NULL),
('city', NULL),
('country', NULL),
('postal_code', NULL)
GO

INSERT INTO [LogSeverity] ([Severity]) VALUES 
('EMERGENCY'),
('ALERT'),
('CRITICAL'),
('ERROR'),
('WARNING'),
('NOTIFICATION'),
('INFO'),
('DEBUG')
GO

INSERT INTO [StaticSearchFilter] ([FilterTypeId], [Key]) VALUES
(1, 'datatekniker'),
(1, 'it-support'),
(1, 'it support'),
(1, 'infrastruktur'),
(1, 'programmering'),
(1, 'programmør'),
(1, 'support'),
(3, 'description'),
(3, 'area body'),
(3, 'article__body'),
(3, 'article_body'),
(3, 'main span'),
(4, 'a[href*="mailto"]'),
(4, 'a[href^="mailto"]'),
(5, 'td'),
(6, 'opslagsdato'),
(7, 'ansøgningsfrist'),
(7, 'ansættelsesdato'),
(8, 'data-'),
(8, 'kommunikationsuddannelsen'),
(9, 'programmering'),
(10, 'adresse')
GO

INSERT INTO [DynamicSearchFilter] ([Key], [CategoryId], [SpecializationId]) VALUES
('netværk', 16, 16),
('supporter', 16, 16),
('it-support', 16, 16),
('supporterelev', 16, 16),
('supporterer', 16, 16),
('it-help', 16, 16),
('it-supporterelev', 16, 16),
('it-supporter', 16, 16),
('supporter-elever', 16, 16),
('first-line', 16, 16),
('first line', 16, 16),
('it-lærling', 16, 16),
('it lærling', 16, 16),
('netværk', 16, 14),
('infrastruktur', 16, 14),
('datateknikerelev', 16, 14),
('speciale i infrastruktur', 16, 14),
('it-datateknikerelev', 16, 14),
('it-support elev', 16, 14),
('it-konsulent', 16, 14),
('it-specialist', 16, 14),
('netværksspecialist', 16, 14),
('programmering', 16, 15),
('programmør', 16, 15),
('programmering lærling', 16, 15),
('programmerings lærling', 16, 15),
('programmør lærling', 16, 15),
('udvikler', 16, 15),
('it-udvikler', 16, 15),
('udvikling', 16, 15),
('database', 16, 15),
('mainframe', 16, 15),
('devops', 16, 15),
('.net', 16, 15),
('dotnet', 16, 15),
('c#', 16, 15),
('java', 16, 15),
('python', 16, 15),
('frontend', 16, 15),
('front-end', 16, 15),
('full-stack', 16, 15),
('full stack', 16, 15),
('fullstack', 16, 15),
('backend', 16, 15),
('back-end', 16, 15),
('back end', 16, 15),
('software', 16, 15),
('medieforskning', 0, 0),
('engineering', 16, 15),
('machine learning', 16, 15),
('android', 16, 15),
('data platform', 16, 15),
('data science', 16, 15),
('data analyst', 16, 15),
('data scientist', 16, 15),
('data science and analystics', 16, 15)
GO