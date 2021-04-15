CREATE DATABASE testdb
 ON PRIMARY
   ( NAME = testdb_data,     
     FILENAME = 'E:\testdb_data.mdf',
     SIZE = 5MB, 
     MAXSIZE = 75MB,
     FILEGROWTH = 3MB )
 LOG ON
   ( NAME = testdb_log,
     FILENAME = 'E:\testdb_log.ldf',
     SIZE = 1MB,
     FILEGROWTH = 20% )
 GO  
 USE testdb
 GO
 CREATE RULE Logical_rule AS @value IN ('Нет', 'Да')
 GO
 CREATE DEFAULT Logical_default AS 'Нет'
 GO
 EXEC sp_addtype Logical, 'char(3)', 'NOT NULL'
 GO
 EXEC sp_bindrule 'Logical_rule', 'Logical'
 GO
 EXEC sp_bindefault 'Logical_default', 'Logical'
 GO
   /* Компания */
 CREATE TABLE Company (	
   CompanyID	INT IDENTITY(1,1) NOT NULL,
   CompanyName	VARCHAR(20) NOT NULL,
   Organization	VARCHAR(10)  NOT NULL,
   CompanySize		INT  NOT NULL,
   CONSTRAINT PK_Company PRIMARY KEY (CompanyName)
 )

  /* Работник */
 CREATE TABLE Employee (	
   EmployeeID	INT IDENTITY(1,1) PRIMARY KEY,
   Surname		VARCHAR(20)  NOT NULL,
   FirstName	VARCHAR(20)  NOT NULL,
   Patronymic		VARCHAR(20)  NOT NULL,
   Date	 	DATETIME  DEFAULT getdate()  NULL,
   Position		VARCHAR(50)  NOT NULL,
   Company	VARCHAR(20)  FOREIGN KEY REFERENCES dbo.Company(CompanyName)
 )