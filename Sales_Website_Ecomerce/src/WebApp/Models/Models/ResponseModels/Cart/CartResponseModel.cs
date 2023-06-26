namespace Models.ResponseModels.Cart
{
    public class CartResponeModel
    {
        public Guid CartID { get; set; }
        public decimal TotalPayment { get; set; } //tổng số tiền cần thanh toán
        public List<CartModel>? lstProduct { get; set; }
    }
}

//
sql

sp_helptext sp_InsertOrderProduct

 CREATE PROC [dbo].[sp_InsertOrder]
    @CustomerID uniqueidentifier,  
	@Status int,
	@OrderID uniqueidentifier
	--@OrderID INT OUT
AS
BEGIN
    -- Insert the record into the table and return the generated Id
    INSERT INTO dbo.Orders(OrderID, CustomerID, Status, CreateDate)
    VALUES (@OrderID,@CustomerID, @Status, GETDATE());

	-- make an actual assignment here...
    --SET @OrderID = SCOPE_IDENTITY();
END

==========================
USE [DB_Guid]
GO

/****** Object:  Table [dbo].[Orders]    Script Date: 6/25/2023 11:09:12 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

drop table Orders
CREATE TABLE [dbo].[Orders](
	[OrderID] [uniqueidentifier] NOT NULL,
	[OrderCode] [varchar](13) NULL,
	[CustomerID] [uniqueidentifier] NOT NULL,
	[DepositAmount] [decimal](18, 2) NULL,
	[TotalPayment] [decimal](18, 2) NULL,
	[Note] [nvarchar](1000) NULL,
	[Status] [int] NULL,
	[CreateDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[OrderID] ASC,
	[OrderCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Orders] ADD  DEFAULT (newid()) FOR [OrderID]
GO

================================
USE [DB_Sang]
GO

/****** Object:  Table [dbo].[OrderProducts]    Script Date: 6/25/2023 11:12:48 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

drop table [OrderProducts]
CREATE TABLE [dbo].[OrderProducts](
	[OrderProductID] [uniqueidentifier] NOT NULL,
	[OrderID] [uniqueidentifier] NOT NULL,
	[ProductID] [uniqueidentifier] NOT NULL,
	[Quantity] [int] NOT NULL,
	--[Price] [decimal](18, 2) NOT NULL,
	[WarehouseID] [uniqueidentifier] NULL,
	[PromoteID] [uniqueidentifier] NULL,
	[CreateDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[OrderProducts]  WITH CHECK ADD FOREIGN KEY([OrderID])
REFERENCES [dbo].[Orders] ([OrderID])
GO

ALTER TABLE [dbo].[OrderProducts]  WITH CHECK ADD FOREIGN KEY([PromoteID])
REFERENCES [dbo].[Price] ([PriceID])
GO

ALTER TABLE [dbo].[OrderProducts]  WITH CHECK ADD FOREIGN KEY([ProductID])
REFERENCES [dbo].[Product] ([ProductID])
GO

ALTER TABLE [dbo].[OrderProducts]  WITH CHECK ADD FOREIGN KEY([WarehouseID])
REFERENCES [dbo].[WareHouse] ([WarehouseID])
GO

////
Text
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
alter PROCEDURE [dbo].[sp_InsertOrderProduct]   
@ListProduct OrderProductType READONLY,
@OrderID uniqueidentifier
--@OrderProductID uniqueidentifier
AS
BEGIN
    -- Insert the records from the temporary table into the actual table
    INSERT INTO dbo.OrderProducts (OrderProductID,OrderID, ProductID, Quantity, WarehouseID, PromoteID, CreateDate)
    SELECT newid(), @OrderID, ProductID, Quantity, WarehouseID, PromoteID, GETDATE() FROM @ListProduct
END


/////////////////////////////////////////
USE [DB_Sang]
GO

/****** Object:  UserDefinedTableType [dbo].[OrderProductType]    Script Date: 6/25/2023 11:35:37 AM ******/
CREATE TYPE [dbo].[OrderProductType] AS TABLE(
	[ProductID] uniqueidentifier NOT NULL,
	[Quantity] int NOT NULL,
	[WarehouseID] uniqueidentifier NOT NULL,
	[PromoteID] uniqueidentifier NOT NULL
)
GO

sp_helptext sp_GetOrderById

////////////////
USE [DB_Guid]
GO

/****** Object:  StoredProcedure [dbo].[SP_Get_AllChildCategoryById]    Script Date: 6/25/2023 12:31:27 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
	CreateBy: ToaiND
	Des: Get categorys theo Id và toàn bộ các danh mục con
*/
alter PROC [dbo].[sp_GetNumberProductsInCart]
	@CartID uniqueidentifier
AS
BEGIN
	SELECT count(CartID) NumberProductsInCart FROM CartProduct
	where Status <> 80 and CartID = @CartID
	group by CartID
END

GO

==========================
USE [DB_Sang]
GO

/****** Object:  Table [dbo].[Notifications]    Script Date: 6/25/2023 3:13:00 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Notifications](
	[NotificationID] uniqueidentifier,
	[Content] [nvarchar](1000) NOT NULL,
	[Note] [nvarchar](1000) NULL,
	[Status] [int] NOT NULL,
	[CreateDate] [datetime] NULL,
	[CreateBy] uniqueidentifier,
	[UpdateDate] [datetime] NULL,
	[UpdateBy] uniqueidentifier,
PRIMARY KEY CLUSTERED 
(
	[NotificationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Notifications] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
================================================================
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
Create PROCEDURE [dbo].[sp_InsertNotification]
	@Content nvarchar(1000),
	@Note nvarchar(1000) ='', 
	@Status int,
	@CreateBy [nvarchar](50),
	@NotificationID uniqueidentifier
AS
BEGIN
    INSERT INTO Notifications (NotificationID,Content, Note, Status, CreateDate, CreateBy)
    VALUES (@NotificationID, @Content, Case when (@Note is null) then '' else @Note end, @Status, GETDATE(), @CreateBy)
END
////////////////////////
alter PROCEDURE [dbo].[sp_UpdateOrder]
	@OrderID uniqueidentifier ,
	@DepositAmount decimal(18, 2)=0.00,
	@Note nvarchar(1000) ='' ,
	@Status int = 0,
	@TotalPayment decimal(18, 2) =0.00
AS
BEGIN
	declare @NoteOld nvarchar(100) ='';
	set @NoteOld = (select Note from Orders where OrderID = @OrderID)--để ghi lại note của tất cả lần update
	UPDATE Orders
		SET 
		DepositAmount = CASE WHEN(@DepositAmount = 0.00) THEN DepositAmount ELSE @DepositAmount END, 
		Note = CASE WHEN(@Note = '') THEN Note ELSE concat(@NoteOld ,'&$%',@Note) END, 
		Status = CASE WHEN(@Status = 0) THEN Status ELSE @Status END,
		UpdateDate = getdate(),
		TotalPayment = CASE WHEN(@TotalPayment = 0.00) THEN TotalPayment ELSE @TotalPayment END
		WHERE OrderID = @OrderID
END;

///////////////////////////
alter PROC [dbo].[sp_GetOrderById]
	@OrderID uniqueidentifier,
	@Status int
AS
BEGIN
	if(@Status=-1) begin
		SELECT * FROM Orders(Nolock) WHERE OrderID = @OrderID and Status <> 15 --15: đơn hàng bị hủy
	end else 
	begin
		SELECT * FROM Orders(Nolock) WHERE Status = @Status
	end
END;
/////////////////////////
alter PROC [dbo].[sp_GetOrderDetailById]
	@OrderID uniqueidentifier
AS
BEGIN
	SELECT p.Name, p.Price, p.ProductID, op.PromoteID, op.Quantity, op.WarehouseID FROM OrderProducts(Nolock) op
	inner join Product(Nolock) p
	on p.ProductID = op.ProductID
	WHERE OrderID = @OrderID
END

/////////////////////////////////
USE [DB_Guid]
GO

/****** Object:  StoredProcedure [dbo].[SP_GetPricesByProductID]    Script Date: 6/25/2023 8:07:58 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
	CreateBy: ToaiND
	Des: Get Prices By ProductID
*/
alter PROC [dbo].[SP_GetPromoteByPromoteID] 
	@PromoteID uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;
	SELECT PriceID,PriceCode,ProductID,PromotePrice,PromotPercent, ExpirationDate,EffectiveDate, IsActive FROM Price(Nolock)
	WHERE PriceID = @PromoteID and ProductID 
			AND GETDATE() >= effectiveDate AND GETDATE()< expirationDate
END
GO





select * from Cart
select * from CartProduct
--select * from ProductStock where WareHouseID = '3FA85F64-5717-4562-B3FC-2C963F66AFA6' and ProductID = '3FA85F64-5717-4562-B3FC-2C963F66AFA6'
select * from Orders
select * from OrderProducts
select * from Notifications
select * from product
select * from Price

sp_helptext sp_UpdateOrder
///
{
  "produtID": 1,
  "customerID": 100,
  "quantity": 2,
  "warehouseID": 2
}

data xóa
173102C9-2912-4F1D-B561-2A532F659C86

addcart
{
  "produtID": "D3F0D078-888C-4104-AECC-1FD91FEA3EFF",
  "customerID": "3fa85f64-5717-4562-b3fc-2c963f66af10",
  "quantity": 1,
  "warehouseID": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "promoteID": "DF7389E3-5A15-4DC5-B6CE-D531F7A11FE5"
}

//xóa là update cart và cartproduct
//update 
  + chỉ xóa 1 cartproduct từ cart và k xóa cart
  + update số lượng

 //'00000000-0000-0000-0000-000000000000'

 thanh toan có xóa lun cart k khi chi mun thanh toán 1 sp

 /////
 f9f6e006-c958-4f11-96c4-126a0b0e3aba
 {
  "cartID": "D23A57D8-FD9D-4972-A03F-F92B7EDD0830",
  "customerID": "3fa85f64-5717-4562-b3fc-2c963f66af10",
  "depositAmount": 15.5,
  "note": "nh11",
  "status": 0
}

{
  "cartID": "D23A57D8-FD9D-4972-A03F-F92B7EDD0830",
  "customerID": "3FA85F64-5717-4562-B3FC-2C963F66AF10",
  "depositAmount": 0,
  "note": "string",
  "status": 0,
  "lstProduct": [
    {
      "productID": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "quantity": 4,
      "wareHouseID": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "promoteID": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
    }
  ]
}

oder: 4252
7ad

su ly code trong table order


uniqueidentifier

delete - create - get 

F9F6E006-C958-4F11-96C4-126A0B0E3ABA

get min chua lam


 //chua lam hold product trong productstock
