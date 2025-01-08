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

-- inserting data in to Category table
insert into Category(c_name) values
('MilkProduct'),
('Produce'),
('Bakery'),
('Drinks')


-- inserting data in to Product table
insert into Product(p_name, c_id, price) values
('Yogurt', 1, 2.50)

-->>>>>>>>>>>>>>>>>>>>>>>> Stored Procedures <<<<<<<<<<<<<<<<<<<<<<<

-->>>>>>>>>>>>>> sp_listCategories
create procedure sp_listCategories
as
begin
	select c_id, c_name from Category
end


-->>>>>>>>>>>>>> sp_listProducts
create procedure sp_listProducts
as
begin	
	select p.p_id, p.p_name,
	c.c_id, c.c_name, p.Price	
	from Product as p
	inner join Category as c on p.c_id = c.c_id
end


-->>>>>>>>>>>>>sp_saveProduct

create procedure sp_saveProduct(
@p_name varchar(50),
@c_id int,
@Price int
)
as
begin
	--set dateformat dmy
	insert into Product(p_name, c_id, Price) values
	(@p_name, @c_id, @Price)
end


-->>>>>>>>>>>>>>>sp_editProduct

create procedure sp_editProduct(
@p_id int,
@p_name varchar(50),
@c_id int,
@Price int
)
as
begin
	--set dateformat dmy
	update Product set
	p_name = @p_name,
	c_id = @c_id,
	Price = @Price
	Where p_id = @p_id
end


-->>>>>>>>>>>>>>>sp_deleteProdduct

create procedure sp_deleteProdduct(
@p_id int
)
as
begin
	delete from Product where p_id = @p_id
end