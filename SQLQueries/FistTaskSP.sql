USE Test

GO

--DROP PROCEDURE sp_GenerateReferenceData
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

USE master