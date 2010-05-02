--- <summary>Converts an empty string to null value</summary>
--- <param name="str">A string value</param>
--- <returns>Null when <paramref name="str"/> is an empty string; <paramref name="str"/> unchanged otherwise</returns>
CREATE FUNCTION [dbo].[EmptyStrToNull](@str nvarchar(MAX)) RETURNS nvarchar(MAX) AS
BEGIN
	RETURN CASE WHEN @str = '' THEN NULL ELSE @str END;
END;