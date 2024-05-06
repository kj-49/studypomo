CREATE TABLE [dbo].[TaskPriority]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Level] NVARCHAR(50) NOT NULL, 
    [DisplayHexColor] VARCHAR(12) NULL
)
