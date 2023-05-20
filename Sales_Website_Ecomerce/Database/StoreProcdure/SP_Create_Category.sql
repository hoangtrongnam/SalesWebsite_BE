USE [Sales_Website_DB]
GO

/****** Object:  StoredProcedure [dbo].[SP_Create_Category]    Script Date: 5/20/2023 1:03:46 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*
	Des: Tạo mới danh mục
*/
CREATE PROC [dbo].[SP_Create_Category]
	@Parent INT,
	@Name NVARCHAR(150),
	@Description NVARCHAR(250),
	@TenantID INT,
	@CreateBy VARCHAR(50)
AS
BEGIN
	Declare @CategoyID INT = 0

	INSERT INTO CategoryProduct
	(
		Parent,
		Name,
		Description,
		TenantID,
		CreateDate,
		CreateBy,
		UpdateDate,
		UpdateBy
	)
	VALUES
	(
		@Parent,
		@Name,
		@Description,
		@TenantID,
		GETDATE(),
		@CreateBy,
		GETDATE(),
		@CreateBy
	)
	SET @CategoyID = @@IDENTITY
	
	--Get category after created
	SELECT ID, Parent,Name,Description,TenantID,CreateBy,CreateDate
		,UpdateBy,UpdateDate FROM CategoryProduct(Nolock) WHERE ID = @CategoyID
END
GO


