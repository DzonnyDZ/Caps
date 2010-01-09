CREATE procedure [dbo].[GetCapsOfPseudoCategory] 
(
	@PseudoCategoryID int,
	@SELECT NVARCHAR(MAX) = '*'
)

AS
BEGIN

DECLARE @Condition NVARCHAR(1024);
SET @Condition = (SELECT Condition FROM dbo.PseudoCategory pc  WHERE pc.PseudoCategoryID = @PseudoCategoryID);

DECLARE @Cmd NVARCHAR(MAX);
SET @Cmd = 'SELECT ' + @SELECT + ' FROM Cap WHERE ' + @Condition + '';

EXEC (@Cmd);


END
