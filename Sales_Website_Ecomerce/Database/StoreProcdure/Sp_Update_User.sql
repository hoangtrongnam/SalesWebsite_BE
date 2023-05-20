USE [Sales_Website_DB]
GO

/****** Object:  StoredProcedure [dbo].[Sp_Update_User]    Script Date: 5/20/2023 1:48:40 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*
	Description: Update Usser
*/
CREATE PROC [dbo].[Sp_Update_User]
	@ID INT,
	@UserName VARCHAR(50),
	@PasswordHasd VARCHAR(250),
	@FullName NVARCHAR(100),
	@EmailAddress VARCHAR(50),
	@PhoneNumber VARCHAR(20),
	@Address NVARCHAR(250),
	@Result INT OUT
AS
BEGIN
	IF (EXISTS(SELECT UserName FROM Users(Nolock) WHERE ID = @ID))
	BEGIN
		UPDATE Users SET
			UserName = CASE WHEN (@UserName IS NULL) THEN UserName ELSE @UserName END,
			PasswordHasd = CASE WHEN (@PasswordHasd IS NULL) THEN PasswordHasd ELSE @PasswordHasd END,
			FullName = CASE WHEN (@FullName IS NULL) THEN FullName ELSE @FullName END,
			EmailAddress = CASE WHEN (@EmailAddress IS NULL) THEN EmailAddress ELSE @EmailAddress END,
			PhoneNumber = CASE WHEN (@PhoneNumber IS NULL) THEN PhoneNumber ELSE @PhoneNumber END,
			Address = CASE WHEN (@Address IS NULL) THEN Address ELSE @Address END,
			UpdateDate = GETDATE()
		WHERE ID = @ID

		SET @Result = @@ROWCOUNT
	END
	--not exist
	ELSE 
		SET @Result = -1
END
GO


