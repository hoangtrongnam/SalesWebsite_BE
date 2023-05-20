USE [Sales_Website_DB]
GO

/****** Object:  StoredProcedure [dbo].[SP_Get_CategoryTenantByID_Or_Name]    Script Date: 5/20/2023 1:47:23 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*
	Des: Get category theo Id hoặc Name tùy vào mục đích sử dụng
*/
CREATE PROC [dbo].[SP_Get_CategoryTenantByID_Or_Name]
	@ID INT,
	@TenantID INT,
	@Parent INT,
	@Name INT
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT ID, Parent,Name,Description,TenantID,CreateBy,CreateDate
		,UpdateBy,UpdateDate FROM CategoryProduct(Nolock)
		WHERE TenantID = @TenantID AND Parent = @Parent
		AND @ID = CASE WHEN(@ID IS NULL) THEN @ID ELSE ID END
		AND @Name = CASE WHEN(@Name IS NULL) THEN @Name ELSE Name END
END
GO


