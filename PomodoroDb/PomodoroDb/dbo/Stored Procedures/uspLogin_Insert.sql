CREATE PROCEDURE [dbo].[uspLogin_Insert]
	@Id	int,
	@Timestamp datetime2,
	@AspNetUsersId int
AS
BEGIN
	INSERT INTO [Login]([Timestamp], AspNetUsersId)
	OUTPUT INSERTED.*
	VALUES (@Timestamp, @AspNetUsersId)
END