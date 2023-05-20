USE [Sales_Website_DB]
GO

/****** Object:  StoredProcedure [dbo].[SP_GetTenantByID]    Script Date: 5/20/2023 1:48:28 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*
	Des: Load Tenant By ID
*/
CREATE PROC [dbo].[SP_GetTenantByID]
	@ID INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT ID, Name, Description FROM Tenant(Nolock) WHERE ID = @ID
END
GO


