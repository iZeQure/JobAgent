USE [JobAgentDB]
GO

INSERT INTO [User]
	([FirstName],[LastName],[Email],[Password],[Salt],[AccessToken],[ConsultantAreaId],[LocationId])
VALUES
	('Udvikler',
	'Konto',
	'udvikler@ja.dk',
	'z4IGQFOG3Ex3LxFsZcL/J6MNpI5uLestVpHpE0Tx400skBGB/QDxx+Hd2S3JKJo5',
	'xjK8Wy17iKFf2c6I/xO09SZuLrg2m9/8nM3GOZfEhhSLlceTbeRR1+m2Y2fKEunQ2QaDCYbyKc4cskNcPw6l3w==', 
	'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOjE1OTkwMjkzMzgsImV4cCI6MTU5OTQ2MTMzOCwiaWF0IjoxNTk5MDI5MzM4LCJpc3MiOiJqb2JhZ2VudC56YmMuZGsiLCJhdWQiOiJqb2JhZ2VudC56YmMuZGsifQ.Fl1__wE8gRiG3yii57A1I6qpIVW5akeAlHtQwc584jE', 
	23,
	4)
GO