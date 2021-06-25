USE [JobAgentDB_v2]
GO

DECLARE @getdate datetime;
SET @getdate = GETDATE();

EXEC [JA.spCreateUser] 3, 4, 'Test', 'Konto', 'udvikler@ja.dk', 'z4IGQFOG3Ex3LxFsZcL/J6MNpI5uLestVpHpE0Tx400skBGB/QDxx+Hd2S3JKJo5', 'xjK8Wy17iKFf2c6I/xO09SZuLrg2m9/8nM3GOZfEhhSLlceTbeRR1+m2Y2fKEunQ2QaDCYbyKc4cskNcPw6l3w==', 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOjE1OTkwMjkzMzgsImV4cCI6MTU5OTQ2MTMzOCwiaWF0IjoxNTk5MDI5MzM4LCJpc3MiOiJqb2JhZ2VudC56YmMuZGsiLCJhdWQiOiJqb2JhZ2VudC56YmMuZGsifQ.Fl1__wE8gRiG3yii57A1I6qpIVW5akeAlHtQwc584jE';
EXEC [JA.spCreateUser] 3, 4, 'No Functionality', 'Account', 'test@ja.dk', '', '', '';
EXEC [JA.spGrantUserArea] 1, 1;
EXEC [JA.spGrantUserArea] 1, 2;
EXEC [JA.spGrantUserArea] 1, 3;
EXEC [JA.spCreateProjectInformation] 'Din Job Agent', @getdate;
EXEC [JA.spCreateProjectInformation] 'Web Crawler', @getdate;
EXEC [JA.spCreateVersionControl] 1, 1, 'd4360e5ec339461cec47ce7ce121ad5b00ce6bb4', 1, 0, 0, @getdate;
EXEC [JA.spCreateVersionControl] 1, 3, '1ce3f296fe6dd740ad1609f4bb15dabddfe89aae', 1, 9, 8, @getdate;
EXEC [JA.spCreateVersionControl] 2, 3, '6c6e59db68d1ed488c95a0df857bc169bee4fc8e', 0, 19, 1, @getdate;
EXEC [JA.spCreateVersionControl] 1, 3, '9190096570a4bdbf4ba0f8c4324b247390f8c855', 0, 20, 3, @getdate;
EXEC [JA.spCreateCompany] 62786515,'DR','Ikke Nogen';
EXEC [JA.spCreateCompany] 00000001, 'Praktikpladsen', 'Staten';
EXEC [JA.spCreateCompany] 00000002, 'Discord', 'DiscordStaff';
EXEC [JA.spCreateJobPage] 1, 'https://www.dr.dk/tjenester/job-widget/';
EXEC [JA.spCreateJobPage] 2, 'https://pms.praktikpladsen.dk/soeg-opslag/0/Data-%20og%20kommunikationsuddannelsen/Datatekniker%20med%20speciale%20i%20programmering';
EXEC [JA.spCreateJobPage] 3, 'https://discord.com/jobs';
EXEC [JA.spCreateJobPage] 2, 'https://pms.praktikpladsen.dk/soeg-opslag/0/Data-%20og%20kommunikationsuddannelsen/Datatekniker%20med%20speciale%20i%20infrastruktur';
EXEC [JA.spCreateJobPage] 2, 'https://pms.praktikpladsen.dk/soeg-opslag/0/Data-%20og%20kommunikationsuddannelsen/IT-supporter';

--INSERT INTO [ProjectInformation] ([Name], [PublishedDateTime]) VALUES
--('Din Job Agent', GETDATE()),
--('Web Crawler', GETDATE())
--GO

--INSERT INTO [VersionControl] ([ProjectInformationId], [ReleaseTypeId], [CommitId], [Major], [Minor], [Patch], [ReleaseDateTime]) VALUES
--(1, 1, 'd4360e5ec339461cec47ce7ce121ad5b00ce6bb4', 1, 0, 0),
--(1, 3, '1ce3f296fe6dd740ad1609f4bb15dabddfe89aae', 1, 9, 8),
--(2, 3, '6c6e59db68d1ed488c95a0df857bc169bee4fc8e', 0, 19, 1),
--(1, 3, '9190096570a4bdbf4ba0f8c4324b247390f8c855', 0, 20, 3)
--GO

--INSERT INTO [Company] ([CVR],[Name],[ContactPerson])
--VALUES 
--(62786515,'DR','Ikke Nogen'),
--(00000001, 'Praktikpladsen', 'Staten'),
--(00000002, 'Discord', 'DiscordStaff');

--INSERT INTO [JobPage] ([CompanyId],[URL])
--VALUES
--(1, 'https://www.dr.dk/tjenester/job-widget/'),
--(2, 'https://pms.praktikpladsen.dk/soeg-opslag/0/Data-%20og%20kommunikationsuddannelsen/Datatekniker%20med%20speciale%20i%20programmering'),
--(3, 'https://discord.com/jobs'),
--(2, 'https://pms.praktikpladsen.dk/soeg-opslag/0/Data-%20og%20kommunikationsuddannelsen/Datatekniker%20med%20speciale%20i%20infrastruktur'),
--(2, 'https://pms.praktikpladsen.dk/soeg-opslag/0/Data-%20og%20kommunikationsuddannelsen/IT-supporter');