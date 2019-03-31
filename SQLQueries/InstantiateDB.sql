CREATE DATABASE Test

GO

USE Test

CREATE TABLE ReferenceData (
	Id INT NOT NULL IDENTITY,
	LookupName NVARCHAR(10),
	CONSTRAINT PK_RererenceData PRIMARY KEY (Id)
)

GO

CREATE TABLE TransactionalData (
	Id INT NOT NULL IDENTITY,
	ReferenceDataId INT NOT NULL,
	DataValue NVARCHAR(5) NOT NULL,
	CONSTRAINT PK_TransactionalData PRIMARY KEY (Id),
	CONSTRAINT FK_TransactionalData_ReferenceData FOREIGN KEY (ReferenceDataId) REFERENCES ReferenceData (Id)
)



GO

CREATE PROCEDURE sp_GenerateReferenceData
@passToFileWithData NVARCHAR(MAX),
@truncateOnStart BIT = 1,
@amountOfRows INT = 1000000
AS
BEGIN
IF @truncateOnStart = 1
BEGIN
	ALTER TABLE TransactionalData DROP FK_TransactionalData_ReferenceData
	TRUNCATE TABLE ReferenceData
	TRUNCATE TABLE TransactionalData
	ALTER TABLE TransactionalData ADD CONSTRAINT FK_TransactionalData_ReferenceData FOREIGN KEY (ReferenceDataId) REFERENCES ReferenceData (Id)
END

ALTER DATABASE Test SET RECOVERY SIMPLE

declare @q nvarchar(MAX);
set @q=
    'BULK INSERT [ReferenceData] FROM ''' + @passToFileWithData + '''
    WITH
    (
		DATAFILETYPE = ''char'',
		FIRSTROW = 1,
		FIELDTERMINATOR = '','',
		ROWTERMINATOR = ''\n'',
		TABLOCK
    )'
exec(@q)

ALTER DATABASE Test SET RECOVERY FULL;

END



GO

CREATE PROCEDURE sp_ImportTransactionalData
@passToFileWithData NVARCHAR(MAX),
@truncateOnStart BIT = 0
AS
BEGIN
IF @truncateOnStart = 1
BEGIN
	TRUNCATE TABLE TransactionalData
END

ALTER DATABASE Test SET RECOVERY SIMPLE

declare @q nvarchar(MAX);
set @q=
    'BULK INSERT TransactionalData FROM ''' + @passToFileWithData + '''
    WITH
    (
		DATAFILETYPE = ''char'',
		FIRSTROW = 1,
		FIELDTERMINATOR = '', '',
		ROWTERMINATOR = ''\n'',
		TABLOCK
    )'
exec(@q)

ALTER DATABASE Test SET RECOVERY FULL;

END

GO

