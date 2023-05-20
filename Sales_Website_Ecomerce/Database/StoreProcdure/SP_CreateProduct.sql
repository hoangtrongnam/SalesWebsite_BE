USE [Sales_Website_DB]
GO

/****** Object:  StoredProcedure [dbo].[SP_CreateProduct]    Script Date: 5/20/2023 1:04:32 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*
	Des: Tạo mới sản phẩm
*/
CREATE PROC [dbo].[SP_CreateProduct]
	@CategoryID INT,
	@Name NVARCHAR(150),
	@Code VARCHAR(50),
	@Description NVARCHAR(250),
	@Quantity INT,
	@StatusID INT,
	@TenantID INT,
	@CreateBy VARCHAR(50),
	@Result INT OUT
AS
BEGIN

	INSERT INTO Product
	(
		CategoryID,
		Name,
		Code,
		Description,
		Quantity,
		StatusID,
		TenantID,
		CreateDate,
		CreateBy,
		UpdateDate,
		UpdateBy
	)
	VALUES
	(
		@CategoryID,
		@Name,
		@Code,
		@Description,
		@Quantity,
		@StatusID,
		@TenantID,
		GETDATE(),
		@CreateBy,
		GETDATE(),
		@CreateBy
	)
	SET @Result = @@IDENTITY
END
GO


