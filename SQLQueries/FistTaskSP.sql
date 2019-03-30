CREATE PROCEDURE sp_GenerateReferenceData
@passToFileWithData NVARCHAR(MAX),
@truncateOnStart BIT = 1,
@amountOfRows INT = 1000000
AS
BEGIN
IF @truncateOnStart = 1
BEGIN
	-- DELETE will be slow on huge amount of data. TRUNCATE should be considered, but then I should REMOVE/ADD all FKs
	DELETE ReferenceData
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