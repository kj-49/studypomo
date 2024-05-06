CREATE TABLE [dbo].[Task]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [AspNetUsersId] INT NOT NULL, 
    [Name] NVARCHAR(50) NOT NULL, 
    [Completed] BIT NOT NULL DEFAULT 0, 
    [DateCreated] DATETIME2 NOT NULL, 
    [DateCompleted] DATETIME2 NULL, 
    [TaskPriorityId] INT NULL, 
    CONSTRAINT [FK_Task_AspNetUsers] FOREIGN KEY ([AspNetUsersId]) REFERENCES [AspNetUsers]([Id]), 
    CONSTRAINT [FK_Task_TaskPriority] FOREIGN KEY ([TaskPriorityId]) REFERENCES [TaskPriority]([Id])
)
