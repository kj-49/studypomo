CREATE TABLE [dbo].[StudySession]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [AspNetUsersId] INT NOT NULL, 
    [Started] DATETIME2 NOT NULL, 
    [Ended] DATETIME2 NOT NULL, 
    [StudyTypeId] INT NULL, 
    CONSTRAINT [FK_StudySession_AspNetUsers] FOREIGN KEY ([AspNetUsersId]) REFERENCES [AspNetUsers]([Id]), 
    CONSTRAINT [FK_StudySession_StudyType] FOREIGN KEY ([StudyTypeId]) REFERENCES [StudyType]([Id])
)
