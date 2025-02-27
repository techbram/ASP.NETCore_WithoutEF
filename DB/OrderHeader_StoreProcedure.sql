CREATE OR ALTER     PROCEDURE [dbo].[sp_saveOrderHeader](

	@ApplicationUserId NVARCHAR(450),
	@OrderDate DATETIME2(7),
	@ShippingDate DATETIME2(7),
	@OrderTotal FLOAT,
	@OrderStatus NVARCHAR(50),
	@PaymentStatus NVARCHAR(50),
	@TrackingNumber NVARCHAR(50),
	@Carrier NVARCHAR(50) ,
	@PaymentDate DATETIME2(7),
	@PaymentDueDate DATETIME2(7),
	@Name NVARCHAR(50),
	@StreetAddress NVARCHAR(50),
	@City NVARCHAR(50),
	@State NVARCHAR(50),
	@PostalCode NVARCHAR(50),
	@PhoneNumber NVARCHAR(50)
)
AS
BEGIN
	

	INSERT INTO OrderHeaders(ApplicationUserId, OrderDate, ShippingDate, OrderTotal, OrderStatus, PaymentStatus, 
	TrackingNumber, Carrier,PaymentDate, PaymentDueDate,  Name, StreetAddress, City, State, PostalCode, PhoneNumber)

	VALUES	( @ApplicationUserId, @OrderDate, @ShippingDate, @OrderTotal, @OrderStatus, @PaymentStatus, 
	@TrackingNumber, @Carrier,@PaymentDate, @PaymentDueDate, @Name, @StreetAddress, @City, @State, @PostalCode, @PhoneNumber)
END
GO
------------------------sp_EditOrderHeader

CREATE OR ALTER  PROCEDURE [dbo].[sp_EditOrderHeader](
    @Id INT,
    @ApplicationUserId NVARCHAR(450),
	@OrderDate DATETIME2(7),
	@ShippingDate DATETIME2(7),
	@OrderTotal FLOAT,
	@OrderStatus NVARCHAR(50),
	@PaymentStatus NVARCHAR(50),
	@TrackingNumber NVARCHAR(50),
	@Carrier NVARCHAR(50) ,
	@PaymentDate DATETIME2(7),
	@PaymentDueDate DATETIME2(7),
	@Name NVARCHAR(50),
	@StreetAddress NVARCHAR(50),
	@City NVARCHAR(50),
	@State NVARCHAR(50),
	@PostalCode NVARCHAR(50),
	@PhoneNumber NVARCHAR(50)

)
AS
BEGIN
	--SET DATEFORMAT dmy
	UPDATE OrderHeaders 

		SET	
		ApplicationUserId = @ApplicationUserId,
		OrderDate = @OrderDate,
		ShippingDate = @ShippingDate,
		OrderTotal = @OrderTotal,
		OrderStatus = @OrderStatus,
		PaymentStatus = @PaymentStatus ,
		TrackingNumber = @TrackingNumber,
		Carrier = @Carrier,
		PaymentDate =  @PaymentDate,
		PaymentDueDate = @PaymentDueDate,
		[Name] = @Name,
		StreetAddress = @StreetAddress,
		City = @City,
		[State] = @State,
		PostalCode = @PostalCode,
		PhoneNumber = @PhoneNumber
	WHERE Id = @Id
END
GO
------------sp_getOrderHeaderId
CREATE OR ALTER PROCEDURE sp_getOrderHeaderId
@OrderHeaderId INT OUTPUT,
@ApplicationUserId NVARCHAR(450)
AS
BEGIN
	SELECT
		o.Id AS OrderHeaderId,
		a.Id AS ApplicationUserId	
	FROM OrderHeaders AS o
	INNER JOIN ApplicationUsers AS a ON a.Id = o.ApplicationUserId
	WHERE 
	 a.Id = @ApplicationUserId
	 AND o.Id = @OrderHeaderId
END
	RETURN @OrderHeaderId
GO
------------
--CREATE OR ALTER PROCEDURE sp_getOrderHeaderId
--    @OrderHeaderId INT OUTPUT,  -- Output parameter for OrderHeaderId
--    @ApplicationUserId NVARCHAR(450)
--AS
--BEGIN
--    -- Query to retrieve the OrderHeaderId based on ApplicationUserId
--    SELECT
--        o.Id AS OrderHeaderId
--    FROM OrderHeaders AS o
--    INNER JOIN ApplicationUsers AS a ON a.Id = o.ApplicationUserId
--    WHERE 
--        a.Id = @ApplicationUserId
--        AND o.Id = @OrderHeaderId;

--    -- If a result is found, set the output parameter to the OrderHeaderId
--    -- The SELECT automatically sets @OrderHeaderId since it's the only value in the result set.
--END
--GO

------------------------sp_saveOrderDetail

CREATE OR ALTER PROCEDURE sp_saveOrderDetail(

	@OrderHeaderId INT,
	@ProductId INT,
	@Count INT,
	@Price FLOAT
)
AS
BEGIN
	INSERT INTO OrderDetails(OrderHeaderId, ProductId, [Count], Price)
	VALUES (@OrderHeaderId, @ProductId, @Count, @Price)
END
GO
-------
CREATE OR ALTER PROCEDURE sp_saveOrderDetail(

	@OrderHeaderId INT,
	@ProductId INT,
	@Count INT,
	@Price FLOAT
)
AS
BEGIN
	DECLARE @_ExistingOrderHeaderId INT
	DECLARE @_ExistingProductId INT
    DECLARE @_ExistingUserId NVARCHAR(450)

	 -- Get the existing cart record (if exists) for the given user and product
    SELECT TOP 1 
        @_ExistingOrderHeaderId = d.OrderHeaderId,
        @_ExistingProductId = d.ProductId
        
    FROM OrderDetails d
    WHERE d.ProductId = @ProductId
    AND d.Id = @OrderHeaderId

IF(OrderHeaderId = @OrderHeaderId)
	INSERT INTO OrderDetails(OrderHeaderId, Price, [Count], Price)
	VALUES (@OrderHeaderId, @ProductId, @Count, @Price)
	 
END
GO

----------------sp_orderDetails
CREATE OR ALTER PROCEDURE sp_orderDetails(
@OrderDetailId INT,
@OrderHeaderId INT OUTPUT,
@ProductId INT,
@Count INT,
@Price FLOAT
)
AS
BEGIN
	SELECT 
	d.ID AS OrderDetailId,
	o.ID AS OrderHeaderId ,
	p.p_id AS ProductId,
	d.[Count],
	d.Price
	FROM OrderDetails AS d
	INNER JOIN Product AS p ON p.p_id = d.ProductId
	INNER JOIN OrderHeaders AS o ON o.Id = d.OrderHeaderId	
	WHERE 
	d.Id = @OrderDetailId
	AND o.Id = d.OrderHeaderId
END
return  @OrderHeaderId
GO

