USE [ZombieCrawlerDB]
GO

-- Title Keys
DECLARE @title_key_id INT = 1
INSERT INTO [AlgorithmKeyWords] ([Key], [KeyTypeId]) VALUES
('datatekniker', @title_key_id), 
('it-support', @title_key_id), 
('infrastruktur', @title_key_id), 
('programmering', @title_key_id), 
('programmør', @title_key_id), 
('support', @title_key_id), 
('it support', @title_key_id)
GO

-- Email Keys
DECLARE @email_key_id INT = 2
INSERT INTO [dbo].[AlgorithmKeyWords] ([Key], [KeyTypeId]) VALUES
('a[href*="mailto"]', @email_key_id), 
('a[href^="mailto"]', @email_key_id)
GO

-- Phone Number Keys
DECLARE @phone_number_key_id INT = 3
INSERT INTO [dbo].[AlgorithmKeyWords] ([Key], [KeyTypeId]) VALUES
('td', @phone_number_key_id)
GO

-- Description Keys
DECLARE @description_key_id INT = 4
INSERT INTO [dbo].[AlgorithmKeyWords] ([Key], [KeyTypeId]) VALUES
('description', @description_key_id), 
('area body', @description_key_id), 
('article__body', @description_key_id), 
('article_body', @description_key_id), 
('main span', @description_key_id)
GO

-- Location Keys
DECLARE @location_key_id INT = 5
INSERT INTO [dbo].[AlgorithmKeyWords] ([Key], [KeyTypeId]) VALUES
('adresse', @location_key_id)
GO

-- Registration Date Keys
DECLARE @registration_date_key_id INT = 6
INSERT INTO [dbo].[AlgorithmKeyWords] ([Key], [KeyTypeId]) VALUES
('opslagsdato', @registration_date_key_id)
GO

-- Deadline Date Keys
DECLARE @deadline_date_key_id INT = 7
INSERT INTO [dbo].[AlgorithmKeyWords] ([Key], [KeyTypeId]) VALUES
('ansøgningsfrist', @deadline_date_key_id), 
('ansættelsesdato', @deadline_date_key_id)
GO

-- Category Keys
DECLARE @category_key_id INT = 8
INSERT INTO [dbo].[AlgorithmKeyWords] ([Key], [KeyTypeId]) VALUES
('data-', @category_key_id), 
('kommunikationsuddannelsen', @category_key_id)
GO

-- Specialization Keys
DECLARE @specialization_key_id INT = 9
INSERT INTO [dbo].[AlgorithmKeyWords] ([Key], [KeyTypeId]) VALUES
('programmering', @specialization_key_id)
GO