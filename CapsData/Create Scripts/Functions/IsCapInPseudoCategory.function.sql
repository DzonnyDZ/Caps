--- <summary>Tests if given Cap (represented by ID) belongs to given PseudoCategory (represented by condition)</summary>
--- <param name="CapID">ID of Cap (<see cref="Cap.CapID"/>) to test</param>
--- <param name="PseudoCategoryCondition">PseudoCategory condition - valid SQL code for WHERE clause</param>
--- <returns>True when <paramref name="PseudoCategoryCondition"/> is tru for Cap <paramref name="CapID"/>; false otherwise</returns>
CREATE FUNCTION [dbo].[IsCapInPseudoCategory]
(
	@CapID INT,
	@PseudoCategoryCondition NVARCHAR(1024)
)
RETURNS BIT
AS
BEGIN
	DECLARE @Cmd NVARCHAR(MAX) = 'SELECT COUNT(*) INTO @Count FROM dbo.CapEx c WHERE c.CapID = @CapID AND (' + @PseudoCategoryCondition + ')'
	DECLARE @Count INT;
	EXEC sp_executesql @Cmd, N'@CapID INT, @Count INT OUT', @CapID, @Count;
	IF @Count > 0 RETURN 1;
	RETURN 0;
END