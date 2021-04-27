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
GO

CREATE TABLE [JobAdvert] (
[VacantJobId] int not null,
[CategoryId] int not null,
[SpecializationId] int not null,
[Title] varchar(100) default 'Oplysninger mangler',
[Summary] varchar(250) default 'Oplysninger mangler',
[Description] varchar(max) default 'Oplysninger mangler',
[Email] varchar(50) default 'Oplysninger mangler',
[PhoneNumber] varchar(25) default 'Oplysninger mangler',
[RegistrationDateTime] datetime default GETDATE(),
[ApplicationDeadlineDateTime] datetime default DATEADD(MONTH, 1, GETDATE()))
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
[URL] varchar(2048) not null,
[CompanyId] int not null)
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