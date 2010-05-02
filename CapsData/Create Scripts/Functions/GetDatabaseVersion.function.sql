--- <summaray>Gets current version number of database</summary>
--- <returns>A string containing a database GUID in braces ({}) followed by database version</returns>
CREATE FUNCTION [dbo].[GetDatabaseVersion] () RETURNS nvarchar(50) AS
BEGIN
    DECLARE @dbGuid NVARCHAR(38) = '{DAFDAE3F-2F0A-4359-81D6-50BA394D72D9}';
    DECLARE @dbVersion NVARCHAR(11) = '0.1.5.0';
    RETURN @dbGuid + @dbversion;
END;