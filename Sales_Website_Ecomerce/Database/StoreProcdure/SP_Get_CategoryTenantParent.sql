USE [Sales_Website_DB]
GO

/****** Object:  StoredProcedure [dbo].[SP_Get_CategoryTenantParent]    Script Date: 5/20/2023 1:47:40 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*
	Des: Get category theo Id hoặc Name tùy vào mục đích sử dụng
*/
CREATE PROC [dbo].[SP_Get_CategoryTenantParent]
	@TenantID INT,
	@Parent INT
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT ID, Parent,Name,Description,TenantID,CreateBy,CreateDate
		,UpdateBy,UpdateDate FROM CategoryProduct(Nolock)
		WHERE TenantID = @TenantID AND
			@Parent = CASE WHEN(@Parent IS NULL) THEN @Parent ELSE Parent END
END
GO


