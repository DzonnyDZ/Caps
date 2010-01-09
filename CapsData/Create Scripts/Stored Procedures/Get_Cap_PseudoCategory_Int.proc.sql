

CREATE PROCEDURE [dbo].[Get_Cap_PseudoCategory_Int]

AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @PseudoCategoryID INT;
	DECLARE @Name NVARCHAR(50);
	DECLARE @Cols VARCHAR(MAX);
	DECLARE @Ret TABLE (CapID INT, PseudoCategoryID INT NOT NULL, Info VARCHAR(MAX) PRIMARY KEY (CapID, PseudoCategoryID));
	DECLARE Cur CURSOR FOR SELECT PseudoCategoryID, Name FROM dbo.PseudoCategory;
	OPEN Cur;
	FETCH NEXT FROM Cur INTO @PseudoCategoryID, @Name;
	WHILE @@FETCH_STATUS = 0 BEGIN
		SET @Cols = 'CapID, ' + CAST(@PseudoCategoryID AS VARCHAR) + ' AS PseudoCategoryID, ''' + @Name + ''' AS Info';
		BEGIN TRY
			INSERT INTO @Ret
				EXEC dbo.GetCapsOfPseudoCategory @PseudoCategoryID, @Cols;
		END TRY
		BEGIN CATCH
			INSERT INTO @Ret VALUES(NULL, @PseudoCategoryID, ERROR_MESSAGE());
		END CATCH
		FETCH NEXT FROM Cur INTO @PseudoCategoryID;
	END;
	CLOSE Cur;
	DEALLOCATE Cur;
	SET NOCOUNT OFF;
	SELECT * FROM @Ret;
END
