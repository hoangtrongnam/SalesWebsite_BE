USE [Sales_Website_DB]
GO

CREATE PROC [dbo].[sp_GetProductById]
	@productId INT
AS
BEGIN
	SELECT Name,Code,Quantity,Price,Description FROM Product(Nolock) WHERE ID = @productId
END

GO