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
[Description] varchar(100) default 'Ingen beskrivelse p� denne rolle.')
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
('Entrepren�r og landbrugsmaskin udd.'),
('Kontoruddannelse med specialer'),
('Handelsuddannelse med specialer'),
('Finansuddannelsen'),
('Eventkoordinator'),
('Detailhandelsuddannelsen med specialer'),
('Skolepraktik'),
('Murer'),
('Tr�fagenes byggeuddannelse'),
('Byggemontage teknikker'),
('VVS-energi'),
('Bygningsmaler'),
('Bager og konditor'),
('Ern�ringsassistent'),
('Gourmetslagter/Industrislagter'),
('Gastronom'),
('Tjener'),
('Receptionist'),
('Data- og kommunikationsuddannelsen'),
('Elektronik- og svagstr�msuddannelsen'),
('Elektriker'),
('Industriteknikeruddannelsen'),
('V�rkt�jsmager'),
('Procesoperat�r'),
('Smed'),
('Landbrugsuddannelsen'),
('Anl�gsgartner'),
('Anl�gsstrukt�r'),
('Serviceassistent'),
('Sikkerhedsvagt'),
('Gourmetslagter'),
('Slagter'),
('Tarmrenser'),
('Styrket praktik i Regionen'),
('F�devare'),
('Merkantil'),
('Industri'),
('Landbrug / Gartner'),
('Transport'),
('Byggeri'),
('SOSU'),
('Ern�ringsassistener'),
('Stem'),
('AUB Merkantil')
GO

INSERT INTO [Location] ([Name])
VALUES
('ZBC Holb�k'),
('ZBC K�ge'),
('ZBC N�stved'),
('ZBC Ringsted'),
('ZBC Roskilde'),
('ZBC Slagelse'),
('ZBC Vordingborg'),
('ZBC Kalundborg')
GO

INSERT INTO [Role] ([Name], [Description])
VALUES
('Konsulent', 'Giver adgang til basale handlinger og funktioner.'),
('Administrator', 'Giver adgang til administrative v�rkt�jer.'),
('Udvikler', 'Fuld system adgang.')
GO

INSERT INTO [Category] ([Name])
VALUES
('Anl�gsgartner'),
('Anl�gsstrukt�r, bygningstrukt�r og brol�gger'),
('Autolakerer'),
('Automatik- og procesuddannelsen'),
('Bager og konditoruddannelsen'),
('Bekl�dningsh�ndv�rker'),
('Beslagsmed'),
('Boligmonteringsuddannelsen'),
('Buschauff�r i kollektiv trafik'),
('Byggemontagetekniker'),
('Bygningsmaler'),
('Bygningssnedker'),
('B�dmekaniker'),
('Cnc-teknikeruddannelsen'),
('Cykel- og motorcykelmekaniker'),
('Data- og kommunikationsuddannelsen'),
('Den p�dagogiske assistentuddannelse'),
('Detailhandel'),
('Digital media'),
('Dyrepasser'),
('Ejendomsservicetekniker'),
('Elektriker'),
('Elektronik- og svagstr�msuddannelsen'),
('Elektronikoperat�r'),
('Entrepren�r- og landbrugsmaskinuddannelsen'),
('Ern�ringsassistent'),
('Eventkoordinator'),
('Film- og tv-produktionsuddannelsen'),
('Finansuddannelsen'),
('Finmekanikeruddannelsen'),
('Fitnessuddannelse'),
('Flytekniker'),
('Forsyningsoperat�r'),
('Fotograf'),
('Fris�r'),
('Gartner'),
('Gastronom'),
('Glarmester'),
('Gourmetslagter'),
('Grafisk tekniker'),
('Greenkeeper'),
('Guld- og s�lvsmedeuddannelsen'),
('Handelsuddannelsen'),
('Havne- og terminaluddannelsen'),
('Hospitalsteknisk assistent'),
('Industrioperat�r'),
('Industrislagter'),
('Industriteknikeruddannelsen'),
('Karrosseriteknikeruddannelsen'),
('Kontoruddannelsen'),
('Kosmetiker'),
('Kranf�rer'),
('K�letekniker'),
('Lager- og terminaluddannelsen'),
('Landbrugsuddannelsen'),
('Lastvognsmekaniker'),
('Lufthavnsuddannelsen'),
('Maritime h�ndv�rksfag'),
('Maskinsnedker'),
('Mediegrafiker'),
('Mejerist'),
('Murer'),
('M�belsnedker og orgelbygger'),
('Ortop�dist'),
('Overfladebehandler'),
('Personvognsmekaniker'),
('Plastmager'),
('Procesoperat�r'),
('Produktions- og montageuddannelsen'),
('Produkt�r'),
('Receptionist'),
('Redderuddannelsen'),
('Serviceassistent'),
('Sikkerhedsvagt'),
('Skibsmekaniker'),
('Skibsmont�r'),
('Skiltetekniker'),
('Skorstensfejer'),
('Skov- og naturtekniker'),
('Smed'),
('Social- og sundhedsuddannelsen'),
('Stenhugger'),
('Stukkat�r'),
('St�beritekniker'),
('Tagd�kker'),
('Tandklinikassistent'),
('Tandtekniker'),
('Tarmrenser'),
('Teater-, udstillings- og eventtekniker'),
('Teknisk designer'),
('Teknisk isolat�r'),
('Tjener'),
('Togklarg�ringsuddannelsen'),
('Tr�fagenes byggeuddannelse'),
('Turistbuschauff�r'),
('Urmager'),
('Vejgodstransportuddannelsen'),
('Veterin�rsygeplejerske'),
('VVS-energiuddannelsen'),
('V�rkt�jsuddannelsen'),
('Webudvikler'),
('Lokomotivf�rer'),
('Fodterapeut')
GO

INSERT INTO [Specialization] ([Name], [CategoryId])
VALUES
('Automatikmont�r', 4),
('Automatiktekniker', 4),
('Automatiseringstekniker', 4),
('Detailbager, h�ndv�rksbager eller konditor', 5),
('Bagv�rker', 5),
('Bekl�dningsh�ndv�rker', 6),
('Tekstil- og bekl�dningsassistent', 6),
('Buschauff�r', 9),
('Buschauff�r i kollektiv trafik', 9),
('K�rselsdisponent', 9),
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
('Entrepren�r- og landbrugsmaskinuddannelsen', 25),
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
('Guld- og s�lvsmed', 42),
('Butiksguldsmed', 42),
('Havne- og terminalarbejder', 44),
('Havnemedhj�lper', 44),
('Industrioperat�r', 46),
('Industrioperat�r-produktivitet', 46),
('Slagter m. speciale', 47),
('Slagter (trin 1)', 47),
('Industritekniker produktion 5', 48),
('Industritekniker maskin 4', 48),
('Industriassistent 3', 48),
('Kranf�rer', 52),
('K�rseldisponent', 52),
('K�letekniker', 53),
('K�leassistent', 53),
('Lager- og terminaldisponent', 54),
('Lageroperat�r � lager- og transportoperat�r', 54),
('Lageroperat�r � lager- og logistikoperat�r', 54),
('Lagermedhj�lper', 54),
('Landbrugsassistent', 55),
('Landbrugsuddannelsen', 55),
('Produktionsleder', 55),
('Agrar�konom', 55),
('Lastvognsmekaniker', 56),
('Lastvognsmont�r', 56),
('Maskinsnedker', 59),
('Produktionsassistent, d�re og vinduer', 59),
('Produktionsassistent, m�bel', 59),
('Mejerist', 61),
('Mejerioperat�r', 61),
('M�belsnedker', 63),
('Orgelbygger', 63),
('Overfladebehandler m. speciale', 65),
('Overfladebehandler', 65),
('Personvognsmekaniker', 66),
('Personvognsmont�r', 66),
('Procesoperat�r', 68),
('Procesoperat�r med speciale i pharma', 68),
('Procesarbejder', 68),
('Skibsmekaniker', 75),
('Skibsmont�r', 76),
('Industrimont�r', 76),
('Stentekniker', 82),
('Stenhugger', 82),
('Teater-, event- og av-teknisk produktionsassistent', 89),
('Teater-, event- og av-tekniker', 89),
('Teknisk designer', 90),
('Tjener', 92),
('Konference- og selskabstjener', 92),
('Klarg�ring og rangering af persontog', 93),
('Klarg�ring og rangering af godstog', 93),
('Turistbuschauff�r', 95),
('K�rselsdisponent', 95),
('Godschauff�r', 97),
('Renovationschauff�r', 97),
('Tankbilchauff�r', 97),
('Flyttechauff�r', 97),
('K�rseldisponent', 97),
('V�rkt�jsuddannelsen', 100),
('V�rkt�jsmager', 100)
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
(1, 'programm�r'),
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
(7, 'ans�gningsfrist'),
(7, 'ans�ttelsesdato'),
(8, 'data-'),
(8, 'kommunikationsuddannelsen'),
(9, 'programmering'),
(10, 'adresse')
GO

INSERT INTO [DynamicSearchFilter] ([Key], [CategoryId], [SpecializationId]) VALUES
('netv�rk', 16, 16),
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
('it-l�rling', 16, 16),
('it l�rling', 16, 16),
('netv�rk', 16, 14),
('infrastruktur', 16, 14),
('datateknikerelev', 16, 14),
('speciale i infrastruktur', 16, 14),
('it-datateknikerelev', 16, 14),
('it-support elev', 16, 14),
('it-konsulent', 16, 14),
('it-specialist', 16, 14),
('netv�rksspecialist', 16, 14),
('programmering', 16, 15),
('programm�r', 16, 15),
('programmering l�rling', 16, 15),
('programmerings l�rling', 16, 15),
('programm�r l�rling', 16, 15),
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