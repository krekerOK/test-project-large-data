CREATE PROCEDURE sp_GenerateReferenceData
@passToFileWithData NVARCHAR(MAX),
@truncateOnStart BIT = 1,
@amountOfRows INT = 1000000
AS
BEGIN
IF @truncateOnStart = 1
BEGIN
	TRUNCATE TABLE ReferenceData
END
declare @q nvarchar(MAX);
set @q=
    'BULK INSERT [ReferenceData] FROM ''' + @passToFileWithData + '''
    WITH
    (
		DATAFILETYPE = ''char'',
		FIRSTROW = 2,
		FIELDTERMINATOR = '','',
		ROWTERMINATOR = ''0x0a''
    )'
exec(@q)
END