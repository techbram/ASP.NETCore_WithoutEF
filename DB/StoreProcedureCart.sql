CREATE PROCEDURE sp_saveOrAddCart(
    @ProductId INT,
    @Count INT,
    @ApplicationUserId NVARCHAR(450)
)
AS
BEGIN
    -- Check if the cart item already exists for the given ApplicationUserId and ProductId
    IF EXISTS (
        SELECT 1
        FROM ShoppingCarts s
        JOIN ApplicationUsers a ON s.ApplicationUserId = a.Id
        JOIN Product p ON s.ProductId = p.p_id
        WHERE s.ApplicationUserId = @ApplicationUserId 
        AND s.ProductId = @ProductId
    )
    BEGIN
        -- If record exists, update the count
        UPDATE ShoppingCarts
        SET Count += @Count
        WHERE ApplicationUserId = @ApplicationUserId 
        AND ProductId = @ProductId
    END
    ELSE
    BEGIN
        -- If record does not exist, insert a new record
        INSERT INTO ShoppingCarts (ProductId, Count, ApplicationUserId)
        VALUES (@ProductId, @Count, @ApplicationUserId)
    END
END
GO
--------------
CREATE OR ALTER PROCEDURE [dbo].[sp_DetailShoppingCart]  ( 
@ShoppingCartId INT,
@ProductId INT,
@ApplicationUserId NVARCHAR(450)
)
AS
BEGIN

	SELECT 
		s.Id AS ShoppingCartId,
		p.p_id AS ProductId,		
	    a.Id AS UserId,   
		s.Count AS ProductCount
		
	FROM ShoppingCarts AS s
	INNER JOIN Product AS p ON p.p_id = s.ProductId
	INNER JOIN ApplicationUsers AS a ON a.Id = s.ApplicationUserId	
	WHERE 
	s.Id = @ShoppingCartId
	AND s.ProductId = @ProductId  -- added check for ProductId
    AND s.ApplicationUserId = @ApplicationUserId -- added check for ApplicationUserId	
END
GO

------------------------------------------->>>>>>>>>>>>>>>>>>>>>>>>
CREATE PROCEDURE sp_editShoppingCart
@Id INT,
@ProductId INT,
@Count INT,
@ApplicationUserId NVARCHAR(450)
AS
BEGIN

	 -- Check if the cart item already exists for the given ApplicationUserId and ProductId
    IF EXISTS (
        SELECT 1
        FROM ShoppingCarts s
        JOIN ApplicationUsers a ON s.ApplicationUserId = a.Id
        JOIN Product p ON s.ProductId = p.p_id
        WHERE s.ApplicationUserId = @ApplicationUserId 
        AND s.ProductId = @ProductId
    )
    BEGIN
        -- If record exists, update the count
        UPDATE ShoppingCarts
        SET Count = @Count
        WHERE ApplicationUserId = @ApplicationUserId 
        AND ProductId = @ProductId
    END
END
GO