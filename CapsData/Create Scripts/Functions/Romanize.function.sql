--- <summary>Converts given Unicode string to Unicode string containing romanized characters</summary>
--- <param name="Str">String to be romenized</param>
--- <returns><paramref name="Str"> with characters for which romanization is know romanized</returns>
--- <remarks>Romanization is based on romanization rules tored in <see cref="dbo.Romanization"/> table</remarks>
CREATE FUNCTION [dbo].[Romanize]
(@Str NVARCHAR(MAX))
RETURNS NVARCHAR(MAX)
AS
BEGIN
    IF @Str IS NULL RETURN NULL;
    IF @Str = N'' RETURN N'';
	DECLARE @String NVARCHAR(MAX) = N'';
    DECLARE @i INT = 0;
    WHILE @i < LEN(@Str) BEGIN
        DECLARE @Char NCHAR(1) = SUBSTRING(@Str,@i+1,1);
        DECLARE @Romanization NVARCHAR(10);
        SET @Romanization = NULL;
        SET @Romanization = (SELECT r.Romanization FROM dbo.Romanization r WHERE r.[Code] = UNICODE(@Char));
        IF @Romanization IS NULL
			SET @String = @String + @Char;
		ELSE
			SET @String = @String + @Romanization;
        SET @i = @i + 1;
    END;
    RETURN @String;
END;