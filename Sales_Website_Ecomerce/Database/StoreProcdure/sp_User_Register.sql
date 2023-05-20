USE [Sales_Website_DB]
GO

/****** Object:  StoredProcedure [dbo].[sp_User_Register]    Script Date: 5/20/2023 1:48:59 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*
	Description: User Register
*/
CREATE PROC [dbo].[sp_User_Register]
	@UserName VARCHAR(50),
	@PasswordHasd VARCHAR(250),
	@FullName NVARCHAR(100),
	@EmailAddress VARCHAR(50),
	@PhoneNumber VARCHAR(20),
	@Address NVARCHAR(250),
	@Result INT OUT
AS
BEGIN
	IF (NOT EXISTS(SELECT UserName FROM Users(Nolock) WHERE Address = @Address))
	BEGIN
		INSERT INTO Users
		(
			UserName,
			PasswordHasd,
			FullName,
			EmailAddress,
			PhoneNumber,
			Address,
			CreateDate,
			UpdateDate
		)
		VALUES
		(
			@UserName,
			@PasswordHasd,
			@FullName,
			@EmailAddress,
			@PhoneNumber,
			@Address,
			GETDATE(),
			GETDATE()
		)

		SET @Result = @@ROWCOUNT
		---Insert Success
		IF(@Result > 0)
			SET @Result = 1
		--Insert Fail
		ELSE 
			SET @Result = 0
	END
	--already exist
	ELSE 
		SET @Result = -1
END
GO


