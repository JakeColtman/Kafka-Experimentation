CREATE TABLE [dbo].[Company]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Name] VARCHAR(255) NOT NULL, 
    [Owner] VARCHAR(255) NOT NULL, 
    [Load_id] INT NOT NULL
)
