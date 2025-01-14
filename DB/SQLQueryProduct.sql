-- Command to create a new database
create database db_AspNetWithoutEF

-- Select database to be used
use db_AspNetWithoutEF

-- Create table Category
create table Category(
	c_id int primary key identity(1, 1),
	c_name varchar(50)
)


--Create table Product
create table Product(
	p_id int primary key identity(1, 1),
	p_name varchar(50),
	c_id int references Category(c_id),
	Price int	
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

-- inserting data in to Category table
insert into Category(c_name) values
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

-->>>>>>>>>>>>>> sp_listCategories
create procedure sp_listCategories
as
begin
	select c_id, c_name from Category
end

--create procedure sp_listProducts
--as
--begin	
--	select p.p_id, p.p_name,
--	c.c_id, c.c_name, p.Price	
--	from Product as p
--	inner join Category as c on p.c_id = c.c_id
--end
-->>>>>>>>>>>>>> sp_listProducts with update the datatype of p.Price
--ALTER PROCEDURE sp_listProducts
--AS
--BEGIN
--    SELECT p.p_id, p.p_name,
--           c.c_id, c.c_name, 
--           CAST(p.Price AS DECIMAL(10, 2)) AS Price
--    FROM Product AS p
--    INNER JOIN Category AS c ON p.c_id = c.c_id
--END;

-->>>>>>>>>>>>>> Alter sp_listProducts with update newly added columns
--ALTER PROCEDURE sp_listProducts
--AS
--BEGIN
--    SELECT p.p_id, 
--           p.p_name,
--           c.c_id, 
--           c.c_name,
--           CAST(p.Price AS DECIMAL(10, 2)) AS Price,
--           p.ImageUrl,           -- New column for image URL
--           p.ProductDetail,      -- New column for product details
--           p.DiscountPrice,      -- New column for discount price
--           p.DateAdded           -- New column for the date the product was added
--    FROM Product AS p
--    INNER JOIN Category AS c ON p.c_id = c.c_id
--END;



----------updat store procedure "sp_listProducts" with date formate
 ALTER PROCEDURE [dbo].[sp_listProducts]
AS
BEGIN
    SET DATEFORMAT dmy
    SELECT p.p_id, 
           p.p_name,
           c.c_id, 
           c.c_name,
           CAST(p.Price AS DECIMAL(10, 2)) AS Price,
           p.ImageUrl,           -- New column for image URL
           p.ProductDetail,      -- New column for product details
           p.DiscountPrice,      -- New column for discount price
		   convert(char(10), p.DateAdded, 103) as 'DateAdded'   -- New column for the date the product was added                 
    FROM Product AS p
    INNER JOIN Category AS c ON p.c_id = c.c_id
END;
GO

---->EXECUTION 
EXEC sp_listProducts



---->>>>>>>>>>>>>sp_saveProduct with update the datatype of p.Price
--ALTER PROCEDURE sp_saveProduct(
--@p_name VARCHAR(50),
--@c_id INT,
--@Price DECIMAL(10,2)
--)
--AS
--BEGIN
--	--set dateformat dmy
--	INSERT INTO Product(p_name, c_id, Price) VALUES
--	(@p_name, @c_id, @Price)
--END
--GO


---sp_saveProduct update with newly fields
ALTER PROCEDURE sp_saveProduct(
@p_name VARCHAR(50),
@c_id INT,
@Price DECIMAL(10,2),
@ImageUrl VARCHAR(100),
@ProductDetail VARCHAR(100),
@DiscountPrice DECIMAL(10,2),
@DateAdded VARCHAR(10)
)
AS
BEGIN
	set dateformat dmy
	INSERT INTO Product(p_name, c_id, Price,ImageUrl, ProductDetail, DiscountPrice,DateAdded) VALUES
	(@p_name, @c_id, @Price, @ImageUrl, @ProductDetail, @DiscountPrice, convert(date, @DateAdded))
END

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


--create procedure sp_saveProduct(
--@p_name varchar(50),
--@c_id int,
--@Price int
--)
--as
--begin
--	--set dateformat dmy
--	insert into Product(p_name, c_id, Price) values
--	(@p_name, @c_id, @Price)
--end


-->>>>>>>>>>>>>>>sp_editProduct with update the datatype of p.Price

--ALTER  PROCEDURE sp_editProduct(
--@p_id INT,
--@p_name VARCHAR(50),
--@c_id INT,
--@Price DECIMAL(10,2)
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

--Latest sp_editProduct code  
ALTER  PROCEDURE [dbo].[sp_editProduct](
@p_id INT,
@p_name VARCHAR(50),
@c_id INT,
@Price DECIMAL(10,2),
@ImageUrl VARCHAR(100),
@ProductDetail VARCHAR(100),
@DiscountPrice DECIMAL(10,2),
@DateAdded VARCHAR(10)
)
as
begin
	set dateformat dmy
	update Product set
	p_name = @p_name,
	c_id = @c_id,
	Price = @Price,
	ImageUrl = @ImageUrl,
	ProductDetail = @ProductDetail,
	DiscountPrice = @DiscountPrice,
	DateAdded = convert(date, @DateAdded)
	Where p_id = @p_id
end
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


-->>>>>>>>>>>>>>>sp_deleteProdduct

create procedure sp_deleteProdduct(
@p_id int
)
as
begin
	delete from Product where p_id = @p_id
end

----------------

select * from Product
exec sp_deleteProdduct 6
exec sp_listProducts