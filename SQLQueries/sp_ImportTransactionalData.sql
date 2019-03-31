USE Test

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
