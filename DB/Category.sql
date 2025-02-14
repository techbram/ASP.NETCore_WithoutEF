--->>>CATEGORY TABLE
	USE [db_AspNetWithoutEF]
	GO

	
-- Create table Category
	CREATE TABLE Category(
		c_id INT PRIMARY KEY IDENTITY(1, 1),
		c_name VARCHAR(50)
	)
	GO

--ADD COLUMN TO CATEGORY TABLE
	ALTER TABLE Category
	ADD 
		CreatedDate DATE
	GO
---Add column to Category "DisplayOrder"
	ALTER TABLE Category
	ADD 
		DisplayOrder int
	GO

--SET VALUE IN COLUMN "CreatedDate"
	UPDATE Category
	SET CreatedDate = GETDATE()
	GO
--Set value in column "DisplayOrder"
	UPDATE Category
	SET DisplayOrder = 5
	WHERE c_name = 'Health Care'
	GO
--------------------------------------->>>> STORE PROCEDURE <<<-----------------------------

-->Store procedure  sp_getListCategory
	CREATE PROCEDURE sp_getListCategory        
	AS
	BEGIN
		SET DATEFORMAT dmy
		SELECT c_id, c_name, DisplayOrder,
		convert(char(10), CreatedDate, 103) as 'CreatedDate'
		from Category
	END
	GO
-------exec sp_getListCategory
	
	EXEC sp_getListCategory
	GO

-->Store procedure for Save Category 
	CREATE PROCEDURE sp_saveCategory(
	
	@c_name VARCHAR(50),
	@DisplayOrder INT,
	@CreatedDate VARCHAR(10)
	)
	AS
	BEGIN
		SET DATEFORMAT dmy
		INSERT INTO Category(c_name, DisplayOrder, CreatedDate ) VALUES
			( @c_name, @DisplayOrder, CONVERT(DATE, @CreatedDate))
	END
	GO 
--EXECUTE sp_saveCategory
   
	DECLARE @RC int
	DECLARE @c_name varchar(50)
	DECLARE @DisplayOrder int
	DECLARE @CreatedDate varchar(10)

    EXECUTE @RC = [dbo].[sp_saveCategory] 
		@c_name = 'Health Care'
	   ,@DisplayOrder  = 5
	   ,@CreatedDate = '10-1-2025';
	GO


----------->>>>Create Store Procedure for Edit

CREATE PROCEDURE sp_editCategory(
@c_id INT,
@c_name VARCHAR(50),
@DisplayOrder INT,
@CreatedDate VARCHAR(10)
)

AS
BEGIN
	SET DATEFORMAT dmy
	UPDATE Category SET	
	c_name = @c_name,
	DisplayOrder = @DisplayOrder,
	CreatedDate = convert(date, @CreatedDate)
	WHERE c_id = @c_id

END
GO

----------->>>>Create Store Procedure sp_deleteCategory

CREATE PROCEDURE sp_deleteCategory(
@c_id INT
)
AS
BEGIN
	DELETE FROM Category WHERE c_id = @c_id
END
GO