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