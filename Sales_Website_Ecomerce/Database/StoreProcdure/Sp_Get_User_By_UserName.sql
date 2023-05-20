USE [Sales_Website_DB]
GO

/****** Object:  StoredProcedure [dbo].[Sp_Get_User_By_UserName]    Script Date: 5/20/2023 1:47:57 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*
	Description: Get User By UserName
*/
CREATE PROC [dbo].[Sp_Get_User_By_UserName]
	@UserName VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT ID,UserName,PasswordHasd,FullName,EmailAddress,PhoneNumber
		,Address,CreateDate,UpdateDate FROM Users(Nolock) WHERE UserName = @UserName
END
GO


