
--------------------------------------------------------------------------------------------------------------------------------
--Increase version
CREATE FUNCTION [dbo].[GetDatabaseVersion] ()
RETURNS nvarchar(50)
AS
BEGIN
declare @dbGuid nvarchar(38) = '{DAFDAE3F-2F0A-4359-81D6-50BA394D72D9}';
declare @dbVersion nvarchar(11) = '0.1.4.0';
return @dbGuid + @dbversion;

END;   
