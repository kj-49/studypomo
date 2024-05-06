CREATE TABLE [dbo].[Login]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Timestamp] DATETIME2 NOT NULL, 
    [AspNetUsersId] INT NOT NULL, 
    CONSTRAINT [FK_Login_AspNetUsers] FOREIGN KEY ([AspNetUsersId]) REFERENCES [AspNetUsers]([Id])
)
