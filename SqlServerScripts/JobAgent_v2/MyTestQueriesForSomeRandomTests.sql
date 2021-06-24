SELECT 
	U.[Email],
	STUFF((	SELECT '; ' + a.[Name]
					FROM [ConsultantArea] c
						INNER JOIN [Area] a ON a.[Id] = c.[AreaId]
					WHERE c.[UserId] = U.Id), 1, 1, '') AS 'Consultant Areas'
FROM [User] U

SELECT DISTINCT
	U.[Id], U.[Email],
	A.[Name] AS 'Consultant Area'
FROM [User] U
JOIN [ConsultantArea] CA ON CA.[UserId] = U.[Id]
JOIN [Area] A ON A.[Id] = CA.[AreaId]

SELECT DISTINCT
U.[Id], U.[Email],
STUFF (
		(SELECT '; ' + A.[Name] 
			FROM [ConsultantArea] CA
			JOIN [Area] A ON A.[Id] = CA.[AreaId]
			WHERE CA.[UserId] = U.[Id]
			FOR XML PATH(''))
		, 1
		, 1
		, ''
	) [Consultant Areas]
FROM [User] U
GROUP BY U.[Id], U.[Email]
ORDER BY 1;