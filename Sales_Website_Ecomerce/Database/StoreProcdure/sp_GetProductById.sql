USE [Sales_Website_DB]
GO

/****** Object:  StoredProcedure [dbo].[sp_GetProductById]    Script Date: 5/20/2023 1:48:13 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[sp_GetProductById]
	@productId INT
AS
BEGIN
	SELECT Name,Code,Quantity,Price,Description FROM Product(Nolock) WHERE ID = @productId
END
GO


