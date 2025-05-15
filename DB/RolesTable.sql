  --USERROLE, ROLES TABLE
  
CREATE TABLE Roles (
    Id NVARCHAR(450) NOT NULL PRIMARY KEY,
    [Name] NVARCHAR(265)  NULL,
   
);

SELECT * FROM Roles

INSERT INTO Roles (Id, Name)
VALUES 
    ('admin123', 'Admin'),
    ('eEmployee123', 'Employee'),
    ('customer123', 'Customer'),
    ('company123', 'Company');


--------------------
	CREATE TABLE UserRoles (
    
    UserId NVARCHAR(450),
    RoleId NVARCHAR(450),
    PRIMARY KEY (UserId, RoleId),
    FOREIGN KEY (UserId) REFERENCES [dbo].[ApplicationUsers](Id),
    FOREIGN KEY (RoleId) REFERENCES [dbo].[Roles](Id)
);

DROP TABLE [dbo].[UserRoles]
DROP TABLE [dbo].[Roles]
SELECT * FROM [dbo].[Roles]



-------------------STORE PROCEDURE [sp_getRoleById]
USE [db_AspNetWithoutEF]
GO

/****** Object:  StoredProcedure [dbo].[sp_getRoleById]    Script Date: 2025-04-30 11:44:19 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_getRoleById]   
@RoleId NVARCHAR(450)
	AS
	BEGIN
	
		SELECT Id AS ApplicationUserId, Name, StreetAddress, City, State, PostalCode, PhoneNumber, CompanyId, Email, PasswordHash
		
		FROM ApplicationUsers
		WHERE Id = @UserId

	END
GO
