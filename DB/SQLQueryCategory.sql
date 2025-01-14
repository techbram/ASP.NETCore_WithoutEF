	--->>>CATEGORY TABLE
	USE [db_AspNetWithoutEF]
	GO

	
	-- Create table Category
	create table Category(
		c_id int primary key identity(1, 1),
		c_name varchar(50)
	)
	GO

	--ADD COLUMN TO CATEGORY TABLE
	ALTER TABLE Category
	ADD 
		CreatedDate DATE
	GO



	--SET VALUE IN COLUMN "CreatedDate"
	UPDATE Category
	SET CreatedDate = GETDATE()
	GO

	--------------------------------------->>>> STORE PROCEDURE <<<-----------------------------

	-->Store procedure  sp_getListCategory
	CREATE PROCEDURE sp_getListCategory        
	AS
	BEGIN
		SET DATEFORMAT dmy
		SELECT c_id, c_name, 
		convert(char(10), CreatedDate, 103) as 'CreatedDate'
		from Category
	END
	GO

	exec sp_getListCategory
	GO

-->Store procedure for Save Category 
	CREATE PROCEDURE sp_saveCategory(
	@c_name varchar(50),
	@CreatedDate varchar(10)
	)
	AS
	BEGIN
		SET DATEFORMAT dmy
		INSERT INTO Category(c_name, CreatedDate) VALUES
			(@c_name, convert(date, @CreatedDate))
	END
	GO 
--EXECUTE sp_saveCategory
   
	DECLARE @RC int
	DECLARE @c_name varchar(50)
	DECLARE @CreatedDate varchar(10)

    EXECUTE @RC = [dbo].[sp_saveCategory] 
		@c_name = 'Health Care'
	   ,@CreatedDate = '10-1-2025';
	GO