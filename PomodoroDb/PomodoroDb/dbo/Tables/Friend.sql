CREATE TABLE [dbo].[Friend]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [AspNetUsersId1] INT NOT NULL, 
    [AspNetUsersId2] INT NULL, 
    [DateCreated] DATETIME2 NOT NULL, 
    CONSTRAINT [AK_Friend_AspNetUsersId1_AspNetUsersId2] UNIQUE ([AspNetUsersId1], [AspNetUsersId2])
)
