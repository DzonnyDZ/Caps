--- <summary>Refreshes all <see cref="Cap"/>-<see cref="PseudoCategory"/> relations in the <see cref="Cap_PseudoCategory_Int"/> table</summary>
CREATE PROCEDURE dbo.AssignPseudocategories
AS
BEGIN
	BEGIN TRANSACTION;
	BEGIN TRY
		DELETE FROM dbo.Cap_PseudoCategory_Int;
		INSERT INTO dbo.Cap_PseudoCategory_Int (CapID,PseudoCategoryID)
			SELECT c.CapID, psc.PseudoCategoryID FROM dbo.Cap c CROSS JOIN dbo.PseudoCategory psc
				WHERE dbo.IsCapInPseudoCategory(c.CapID,psc.Condition) = 1
		COMMIT;
	END TRY
	BEGIN CATCH
		ROLLBACK;
		EXEC usp_RethrowError;
		RETURN;
	END CATCH;			
END