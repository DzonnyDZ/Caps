
CREATE FUNCTION [dbo].[EmptyStrToNull] 
(
	-- Add the parameters for the function here
	@str nvarchar(MAX) 
)
RETURNS nvarchar(MAX)
AS
BEGIN
	

	return case when  @str = '' then null else @str end;

END
