USE db_AspNetWithoutEF
GO


CREATE TABLE [dbo].[ShoppingCarts](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [ProductId] [int] NOT NULL,
    [Count] [int] NOT NULL,
    [ApplicationUserId] [nvarchar](450) NOT NULL,
    CONSTRAINT [PK_ShoppingCarts] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ShoppingCarts_Product_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product]([p_id]),
    CONSTRAINT [FK_ShoppingCarts_ApplicationUsers_ApplicationUserId] FOREIGN KEY ([ApplicationUserId]) REFERENCES [dbo].[ApplicationUsers]([Id])
);

------->>>>STORE PROCEDURE FOR sp_ListShoppingCart  ----<<<Do not need this STORE PROCEDURE


CREATE PROCEDURE sp_listShoppingCart 

AS
BEGIN
	SELECT 
		s.Id,	
		p.p_id AS 'ProductId',
		s.Count,					
	    a.Id AS 'ApplicationUserId'  

	FROM ShoppingCarts AS s
	INNER JOIN Product AS p ON p.p_id = s.ProductId
	INNER JOIN ApplicationUsers AS a ON a.Id = s.ApplicationUserId	
	
END
GO

EXEC sp_listShoppingCart

------->>>STORE PROCEDURE FOR sp_SaveCart
CREATE PROCEDURE sp_saveCart(

	@ProductId INT,
	@Count INT,
	@ApplicationUserId NVARCHAR(450)
)
AS 
BEGIN
	INSERT INTO ShoppingCarts(ProductId, Count, ApplicationUserId )
	VALUES (@ProductId, @Count, @ApplicationUserId)	
END
GO
----------STORE PROCEDURE sp_editCart
CREATE PROCEDURE sp_editCart
@Id INT,
@ProductId INT,
@Count INT,
@ApplicationUserId NVARCHAR(450)
AS
BEGIN
	UPDATE ShoppingCarts SET
	
	ProductId = @ProductId,
	Count += @Count,
	ApplicationUserId = @ApplicationUserId
	WHERE Id = @Id
END
GO
----------------------
CREATE PROCEDURE sp_editShoppingCart
@Id INT,
@ProductId INT,
@Count INT,
@ApplicationUserId NVARCHAR(450)
AS
BEGIN
IF EXISTS (
        SELECT 
		s.Id,
		a.Id,
		p.p_id,
		s.Count

        FROM ShoppingCarts s
        JOIN ApplicationUsers a ON s.ApplicationUserId = a.Id
        JOIN Product p ON s.ProductId = p.p_id
        WHERE s.Id = @Id
    )
	BEGIN
	 UPDATE ShoppingCarts
        SET Count = Count + @Count
        WHERE ApplicationUserId = @ApplicationUserId 
        AND ProductId = @ProductId
		END
END
GO

------->>>>>STORE PROCEDURE FOR sp_DetailShoppingCart----<<<Do not need this STORE PROCEDURE
CREATE PROCEDURE sp_DetailShoppingCart   
@ShoppingCartId INT,
@ProductId INT,
@ApplicationUserId NVARCHAR(450)

AS
BEGIN
	SELECT 
		s.Id,
		p.p_id,
		p.p_name,
		p.Price,
		p.ImageUrl,           -- New column for image URL
		p.ProductDetail,      -- New column for product details
		p.DiscountPrice,      -- New column for discount price
	    convert(char(10), p.DateAdded, 103) as 'DateAdded' ,  -- New column for the date the product was added 
	    p.ProductNumber,
	    p.Originated,
		p.Price50,
	    p.Price100,
	    a.Id,   
		s.Count
		
	FROM ShoppingCarts AS s
	INNER JOIN Product AS p ON p.p_id = s.ProductId
	INNER JOIN ApplicationUsers AS a ON a.Id = s.ApplicationUserId	
	WHERE s.Id = @ShoppingCartId
	AND s.ProductId = @ProductId  -- added check for ProductId
    AND s.ApplicationUserId = @ApplicationUserId -- added check for ApplicationUserId	
END
GO
-----------------------
CREATE OR ALTER   PROCEDURE [dbo].[sp_DetailCart]   
@ShoppingCartId INT
AS
BEGIN
	SELECT 
		s.Id AS ShoppingCartId, 
		p.p_id AS ProductId,		
	    s.Count,
		a.Id AS ApplicationUserId
		 FROM ShoppingCarts AS s
	INNER JOIN Product AS p ON p.p_id = s.ProductId
	INNER JOIN ApplicationUsers AS a ON a.Id = s.ApplicationUserId	
	WHERE 
	s.Id = @ShoppingCartId
	
END
GO
EXEC sp_DetailCart 21
