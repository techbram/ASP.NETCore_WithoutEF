-- Command to create a new database
CREATE DATABASE db_AspNetWithoutEF

-- Select database to be used
USE db_AspNetWithoutEF

-- Create table Category
CREATE TABLE Category(
	c_id INT PRIMARY KEY IDENTITY(1, 1),
	c_name VARCHAR(50)
)


--Create table Product
CREATE TABLE Product(
	p_id INT PRIMARY KEY IDENTITY(1, 1),
	p_name VARCHAR(50),
	c_id INT REFERENCES Category(c_id),
	Price INT	
)

--->>>Change datatype for Price from int to double in Product Table 
ALTER TABLE Product
ALTER COLUMN Price decimal(10,2)


--Alter table Product with adding columns
ALTER TABLE Product
ADD 
ImageUrl VARCHAR(100),
ProductDetail VARCHAR(100),
DiscountPrice DECIMAL(10,2),
DateAdded DATE;


ALTER TABLE [dbo].[Product]
ADD
	[ProductNumber] [nvarchar](max),
	[Originated] [nvarchar](max) ,
	[Price50] [float] ,
	[Price100] [float];
	
-- inserting data in to Category table
INSERT INTO Category(c_name) VALUES
('MilkProduct'),
('Produce'),
('Bakery'),
('Drinks')


-- inserting data in to Product table
INSERT INTO Product(p_name, c_id, price, ImageUrl, ProductDetail, DiscountPrice, DateAdded) VALUES
('Yogurt', 1, 2.50, 'http://example.com/yogurt.jpg', 'Creamy and delicious yogurt', 1.99, getdate())

---Updating data in to Product table with new fields
UPDATE Product
SET 
    ImageUrl = 'http://example.com/yogurt.jpg', 
    ProductDetail = 'Creamy and delicious yogurt', 
    DiscountPrice = 1.99, 
    DateAdded = getdate()
WHERE p_name = 'Yogurt';
---for other product
UPDATE Product
SET 
    ImageUrl = 'http://example.com/broccoli.jpg', 
    ProductDetail = 'Fresh and green Broccoli', 
    DiscountPrice = 1.33, 
    DateAdded = getdate()
WHERE p_name = 'Broccoli';
-----
UPDATE Product
SET 
    ImageUrl = 'http://example.com/bread.jpg', 
    ProductDetail = 'Fresh Whole Wheat Bread', 
    DiscountPrice = 0.0, 
    DateAdded = getdate()
WHERE p_name = 'Whole Wheat Bread';

-->>>>>>>>>>>>>>>>>>>>>>>> Stored Procedures <<<<<<<<<<<<<<<<<<<<<<<

----------CREATE STORE PROCEDURE "sp_listProducts"
CREATE PROCEDURE [dbo].[sp_listProducts]
AS
BEGIN
    SET DATEFORMAT dmy
    SELECT p.p_id, 
           p.p_name,
           c.c_id, 
           c.c_name,
           p.Price,
           p.ImageUrl,           -- New column for image URL
           p.ProductDetail,      -- New column for product details
           p.DiscountPrice,      -- New column for discount price
		   convert(char(10), p.DateAdded, 103) as 'DateAdded' ,  -- New column for the date the product was added 
		   p.ProductNumber,
		   p.Originated,
		   p.Price50,
		   p.Price100
    FROM Product AS p
    INNER JOIN Category AS c ON p.c_id = c.c_id
END;
GO

---->EXECUTION 
EXEC sp_listProducts


--->>>>>CREATE PROCEDURE sp_saveProduct 
CREATE PROCEDURE sp_saveProduct(

	
	@p_name VARCHAR(50),
	@c_id INT,
	@Price DECIMAL(10,2),
	@ImageUrl VARCHAR(100),
	@ProductDetail VARCHAR(100),
	@DiscountPrice DECIMAL(10,2),
	@DateAdded VARCHAR(10),
	@ProductNumber NVARCHAR(MAX),
	@Originated NVARCHAR(MAX),
	@Price50 FLOAT,
	@Price100 FLOAT
)

AS
BEGIN
	SET DATEFORMAT dmy
	INSERT INTO Product(p_name, c_id, Price, ImageUrl, ProductDetail, DiscountPrice, DateAdded, ProductNumber, Originated, Price50, Price100)
	VALUES	( @p_name, @c_id, @Price, @ImageUrl, @ProductDetail, @DiscountPrice, convert(date, @DateAdded), @ProductNumber, @Originated, @Price50, @Price100)
END
GO

---->EXECUTE sp_saveProduct
DECLARE @RC int
DECLARE @p_name varchar(50)
DECLARE @c_id int
DECLARE @Price decimal(10,2)
DECLARE @ImageUrl varchar(100)
DECLARE @ProductDetail varchar(100)
DECLARE @DiscountPrice decimal(10,2)
DECLARE @DateAdded varchar(10)

EXECUTE @RC = [dbo].[sp_saveProduct] 
   @p_name =  'Whole Wheat Bread'
  ,@c_id = 3
  ,@Price = 2.25
  ,@ImageUrl = 'http://example.com/bread.jpg'
  ,@ProductDetail = 'Fresh Whole Wheat Bread'
  ,@DiscountPrice = 2.00
  ,@DateAdded = '14-1-25'
GO



-----------STORE PROCEDURE sp_editProduct
CREATE  PROCEDURE [dbo].[sp_editProduct](
@p_id INT,
@p_name VARCHAR(50),
@c_id INT,
@Price DECIMAL(10,2),
@ImageUrl VARCHAR(100),
@ProductDetail VARCHAR(100),
@DiscountPrice DECIMAL(10,2),
@DateAdded VARCHAR(10),
@ProductNumber NVARCHAR(MAX),
@Originated NVARCHAR(MAX),
@Price50 FLOAT,
@Price100 FLOAT

)
AS
BEGIN
	SET DATEFORMAT dmy
	UPDATE Product SET
	p_name = @p_name,
	c_id = @c_id,
	Price = @Price,
	ImageUrl = @ImageUrl,
	ProductDetail = @ProductDetail,
	DiscountPrice = @DiscountPrice,
	DateAdded = convert(date, @DateAdded),
	ProductNumber = @ProductNumber,
	Originated = @Originated,
	Price50 = @Price50,
	Price100 = @Price100
	WHERE p_id = @p_id
END
GO

--create procedure sp_editProduct(
--@p_id int,
--@p_name varchar(50),
--@c_id int,
--@Price int
--)
--as
--begin
--	--set dateformat dmy
--	update Product set
--	p_name = @p_name,
--	c_id = @c_id,
--	Price = @Price
--	Where p_id = @p_id
--end

----------->>>>CREATE PROCEDURE sp_detailProduct
--CREATE PROCEDURE sp_detailProduct(
--@p_id int
--)
--AS
--BEGIN
--	SELECT p.p_id, 
--           p.p_name,
--           c.c_id, 
--           c.c_name,
--           CAST(p.Price AS DECIMAL(10, 2)) AS Price,
--           p.ImageUrl,           
--           p.ProductDetail,      
--           p.DiscountPrice,     
--		   p.ProductNumber,
--		   p.Originated,
--		   p.Price50,
--		   p.Price100          
--    FROM Product AS p
--    INNER JOIN Category AS c ON p.c_id = c.c_id
--	Where  p.p_id = @p_id
--END;
--GO
------------------------------------------------
CREATE PROCEDURE sp_detailProduct
    @ProductId INT
AS
BEGIN
    -- Check if the provided CompanyId is valid
    IF @ProductId IS NULL OR @ProductId <= 0
    BEGIN
        PRINT 'Invalid Company ID';
        RETURN;
    END

    -- Retrieve company details based on the provided CompanyId
    SELECT 
       p.p_id, 
           p.p_name,
           c.c_id, 
           c.c_name,
           CAST(p.Price AS DECIMAL(10, 2)) AS Price,
           p.ImageUrl,           
           p.ProductDetail,      
           p.DiscountPrice,  
		   p.ProductNumber,
		   p.Originated,
		   p.Price50,
		   p.Price100  
   FROM Product AS p
   INNER JOIN Category AS c ON p.c_id = c.c_id
   WHERE p.p_id = @ProductId;
END;
GO

EXEC sp_detailProduct 8
GO

-->>>>>>>>>>>>>>>sp_deleteProdduct

CREATE PROCEDURE sp_deleteProdduct(
@p_id INT
)
AS
BEGIN
	DELETE FROM Product WHERE p_id = @p_id
END

----------------

select * from Product
exec sp_deleteProdduct 6
exec sp_listProducts