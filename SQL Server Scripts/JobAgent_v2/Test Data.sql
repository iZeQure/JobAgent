USE [JobAgentDB_v2]
GO

INSERT INTO [Company] ([CVR],[Name],[ContactPerson])
VALUES 
(62786515,'DR','Ikke Nogen'),
(00000001, 'Praktikpladsen', 'Staten'),
(00000002, 'Discord', 'DiscordStaff');

INSERT INTO [JobPage] ([CompanyId],[URL])
VALUES
(1, 'https://www.dr.dk/tjenester/job-widget/'),
(2, 'https://pms.praktikpladsen.dk/soeg-opslag/0/Data-%20og%20kommunikationsuddannelsen/Datatekniker%20med%20speciale%20i%20programmering'),
(3, 'https://discord.com/jobs'),
(2, 'https://pms.praktikpladsen.dk/soeg-opslag/0/Data-%20og%20kommunikationsuddannelsen/Datatekniker%20med%20speciale%20i%20infrastruktur'),
(2, 'https://pms.praktikpladsen.dk/soeg-opslag/0/Data-%20og%20kommunikationsuddannelsen/IT-supporter');

INSERT INTO [Version] ([HashId], [SystemName], [ReleaseTypeId], [Major], [Minor], [Patch]) VALUES 
('d4360e5ec339461cec47ce7ce121ad5b00ce6bb4', 'Din Job Agent', 1, 1, 0, 0),
('1ce3f296fe6dd740ad1609f4bb15dabddfe89aae', 'Din Job Agent', 3, 1, 9, 8),
('6c6e59db68d1ed488c95a0df857bc169bee4fc8e', 'Zombie', 3, 0, 19, 1),
('9190096570a4bdbf4ba0f8c4324b247390f8c855', 'Zombie', 3, 0, 20, 3)
GO