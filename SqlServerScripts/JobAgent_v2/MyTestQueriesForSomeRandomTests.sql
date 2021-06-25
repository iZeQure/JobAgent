USE [JobAgentDB_v2]
GO

--SELECT 
--	U.[Email],
--	STUFF((	SELECT '; ' + a.[Name]
--					FROM [ConsultantArea] c
--						INNER JOIN [Area] a ON a.[Id] = c.[AreaId]
--					WHERE c.[UserId] = U.Id), 1, 1, '') AS 'Consultant Areas'
--FROM [User] U

--SELECT DISTINCT
--	U.[Id], U.[Email],
--	A.[Name] AS 'Consultant Area'
--FROM [User] U
--JOIN [ConsultantArea] CA ON CA.[UserId] = U.[Id]
--JOIN [Area] A ON A.[Id] = CA.[AreaId]

SELECT DISTINCT
U.[Id], U.[Email],
ISNULL(STUFF (
		(SELECT ';' + A.[Name] 
			FROM [ConsultantArea] CA
			FULL JOIN [Area] A ON A.[Id] = CA.[AreaId]
			WHERE CA.[UserId] = U.[Id]
			FOR XML PATH(''))
		, 1
		, 1
		, ''
	), '') [Consultant Areas]
FROM [User] U
GROUP BY U.[Id], U.[Email]
ORDER BY U.[Id];

--SELECT DISTINCT
--	U.[Id],

--	SUBSTRING((
--		SELECT ';' + A.[Name] FROM [ConsultantArea] CA
--		LEFT JOIN [Area] A ON A.[Id] = CA.[AreaId]
--		WHERE CA.[UserId] = U.[Id]
--		FOR XML PATH('')
--	), 0, 1000000000) [Constultant Areas]
--FROM [User] U

--SELECT U.[Id],U.[Email], CA.[AreaId] FROM [User] U
--LEFT JOIN [ConsultantArea] CA ON CA.[UserId] = U.[Id]