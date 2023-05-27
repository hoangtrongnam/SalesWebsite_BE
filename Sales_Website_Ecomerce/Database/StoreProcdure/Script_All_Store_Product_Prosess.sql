USE [Sales_Website_DB]
GO
/****** Object:  StoredProcedure [dbo].[SP_Create_Category]    Script Date: 5/27/2023 4:55:45 PM ******/
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
/****** Object:  StoredProcedure [dbo].[SP_CreateImage]    Script Date: 5/27/2023 4:55:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
	Des: Tạo nhiều hình ảnh sản phẩm
*/
CREATE PROC [dbo].[SP_CreateImage]
	@ListImage dbo.ImageType READONLY,
	@Result INT OUT
AS
BEGIN
	INSERT INTO Image(ProductID, Name, Type, Url, Description,
		SortOrder,CreateDate,CreateBy,UpdateDate,UpdateBy)
	SELECT ProductID,Name,Type,Url,Description,SortOrder
		,GETDATE(),CreateBy,GETDATE(),CreateBy FROM @ListImage

	SET @Result = @@ROWCOUNT
END
GO
/****** Object:  StoredProcedure [dbo].[SP_CreatePrice]    Script Date: 5/27/2023 4:55:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
	Des: Tạo nhiều đơn giá
*/
CREATE PROC [dbo].[SP_CreatePrice]
	@ListPrice dbo.PriceType READONLY,
	@Result INT OUT
AS
BEGIN
	INSERT INTO Price(ProductID, Price, PromotePrice, PromotPercent, ExpirationDate,
		EffectiveDate,CreateDate,CreateBy,UpdateDate,UpdateBy)
	SELECT ProductID,Price,PromotePrice,PromotPercent,ExpirationDate,EffectiveDate
		,GETDATE(),CreateBy,GETDATE(),CreateBy FROM @ListPrice

	SET @Result = @@ROWCOUNT
END
GO
/****** Object:  StoredProcedure [dbo].[SP_CreateProduct]    Script Date: 5/27/2023 4:55:45 PM ******/
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
/****** Object:  StoredProcedure [dbo].[SP_Get_CategoryTenantByID_Or_Name]    Script Date: 5/27/2023 4:55:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
	Des: Get category theo Id hoặc Name tùy vào mục đích sử dụng
*/
CREATE PROC [dbo].[SP_Get_CategoryTenantByID_Or_Name]
	@ID INT = NULL,
	@TenantID INT,
	@Parent INT = NULL,
	@Name NVARCHAR(150) = NULL
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT ID, Parent,Name,Description,TenantID,CreateBy,CreateDate
		,UpdateBy,UpdateDate FROM CategoryProduct(Nolock)
	WHERE TenantID = @TenantID 
		AND Parent = CASE WHEN @Parent IS NULL THEN Parent ELSE @Parent END
		AND ID = CASE WHEN @ID IS NULL THEN ID ELSE @ID END
		AND Name = CASE WHEN @Name IS NULL THEN Name ELSE @Name END
END
GO
/****** Object:  StoredProcedure [dbo].[SP_Get_CategoryTenantParent]    Script Date: 5/27/2023 4:55:45 PM ******/
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
/****** Object:  StoredProcedure [dbo].[SP_GetImagesByProductID]    Script Date: 5/27/2023 4:55:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
	Des: Get Images By ProductID
*/
CREATE PROC [dbo].[SP_GetImagesByProductID] 
	@ProductID INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT ID, ProductID,Name,Type,Url,Description,SortOrder FROM Image(Nolock) WHERE ProductID = @ProductID
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetPricesByProductID]    Script Date: 5/27/2023 4:55:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
	Des: Get Prices By ProductID
*/
CREATE PROC [dbo].[SP_GetPricesByProductID] 
	@ProductID INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT ID, ProductID,Price,PromotePrice,PromotPercent, ExpirationDate,EffectiveDate FROM Price(Nolock) WHERE ProductID = @ProductID
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetProductByID]    Script Date: 5/27/2023 4:55:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
	Des: Get Product By Id
*/
CREATE PROC [dbo].[SP_GetProductByID] 
	@ProductID INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT  ID, Name, Code, Description, Quantity,StatusID,TenantID,
		CreateDate,CreateBy,UpdateDate,UpdateBy FROM Product(Nolock)
	WHERE ID = @ProductID
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetTenantByID]    Script Date: 5/27/2023 4:55:45 PM ******/
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
