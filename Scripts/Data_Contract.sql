USE [JobAgentDB]
GO

DECLARE @RegisteredDate Date
SET @RegisteredDate = CURRENT_TIMESTAMP

DECLARE @ExpiryDate Date
SET @ExpiryDate = DATEADD(YEAR, 5, @RegisteredDate)

INSERT INTO [Contract] (
[ContactPerson], [ContractName], [ExpiryDate], [RegisteredDate], [SignedByUserId], [CompanyId]
)
VALUES 
('Alpha', 'alpha.pdf', @ExpiryDate, @RegisteredDate, 1, 1),
('Beta', 'beta.pdf', @ExpiryDate, @RegisteredDate, 1, 2),
('Gamma', 'gamma.pdf', @ExpiryDate, @RegisteredDate, 1, 3),
('Delta', 'delta.pdf', @ExpiryDate, @RegisteredDate, 1, 4)
GO	