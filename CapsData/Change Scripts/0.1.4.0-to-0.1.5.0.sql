BEGIN TRANSACTION;
--------------------------- Cap_PseudoCategory_Int ---------------------------------------------------------------------
PRINT 'CREATE TABLE dbo.Cap_PseudoCategory_Int';
GO
CREATE TABLE dbo.Cap_PseudoCategory_Int (
	CapID int NOT NULL,
	PseudoCategoryID int NOT NULL);
GO
ALTER TABLE dbo.Cap_PseudoCategory_Int ADD CONSTRAINT PK_Cap_PseudoCategory_Int PRIMARY KEY(CapID,PseudoCategoryID);
GO
ALTER TABLE dbo.Cap_PseudoCategory_Int ADD CONSTRAINT FK_Cap_PseudoCategory_Int_Cap FOREIGN KEY (CapID)
	REFERENCES dbo.Cap (CapID) ON DELETE CASCADE;
GO
ALTER TABLE dbo.Cap_PseudoCategory_Int ADD CONSTRAINT FK_Cap_PseudoCategory_Int_PseudoCategory FOREIGN KEY (PseudoCategoryID)
	REFERENCES dbo.PseudoCategory (PseudoCategoryID) ON DELETE CASCADE;
GO	 
------------------------- usp_RethrowError ----------------------------------------------------------------------------
PRINT 'CREATE PROCEDURE usp_RethrowError';
GO
CREATE PROCEDURE usp_RethrowError AS
    -- Return if there is no error information to retrieve.
    IF ERROR_NUMBER() IS NULL
        RETURN;

    DECLARE 
        @ErrorMessage    NVARCHAR(4000),
        @ErrorNumber     INT,
        @ErrorSeverity   INT,
        @ErrorState      INT,
        @ErrorLine       INT,
        @ErrorProcedure  NVARCHAR(200);

    -- Assign variables to error-handling functions that 
    -- capture information for RAISERROR.
    SELECT 
        @ErrorNumber = ERROR_NUMBER(),
        @ErrorSeverity = ERROR_SEVERITY(),
        @ErrorState = ERROR_STATE(),
        @ErrorLine = ERROR_LINE(),
        @ErrorProcedure = ISNULL(ERROR_PROCEDURE(), '-');

    -- Build the message string that will contain original
    -- error information.
    SELECT @ErrorMessage = 
        N'Error %d, Level %d, State %d, Procedure %s, Line %d, ' + 
            'Message: '+ ERROR_MESSAGE();

    -- Raise an error: msg_str parameter of RAISERROR will contain
    -- the original error information.
    RAISERROR 
        (
        @ErrorMessage, 
        @ErrorSeverity, 
        1,               
        @ErrorNumber,    -- parameter: original error number.
        @ErrorSeverity,  -- parameter: original error severity.
        @ErrorState,     -- parameter: original error state.
        @ErrorProcedure, -- parameter: original error procedure name.
        @ErrorLine       -- parameter: original error line number.
        );
GO

---------------------------- Dependency --------------------------------------------------------------------------------
PRINT 'CREATE TABLE dbo.Dependency';
GO
CREATE TABLE dbo.Dependency(
	DependencyID int NOT NULL IDENTITY (1, 1),
	PseudoCategoryID int NOT NULL,
	[Table] nvarchar(255) NOT NULL,
	[Column] nvarchar(255) NULL);
GO
ALTER TABLE dbo.Dependency ADD CONSTRAINT PK_Dependency PRIMARY KEY (DependencyID);
GO
ALTER TABLE dbo.Dependency ADD CONSTRAINT FK_Dependency_PseudoCategory FOREIGN KEY (PseudoCategoryID)
	REFERENCES dbo.PseudoCategory (PseudoCategoryID) ON DELETE  CASCADE ;	
GO
ALTER TABLE [dbo].Dependency WITH CHECK ADD CONSTRAINT [CHK_Dependency_NoEmptyString]
	CHECK ([Table] <> '' AND [Column] <> '');
GO
ALTER TABLE [dbo].Dependency CHECK CONSTRAINT [CHK_Dependency_NoEmptyString];
GO
--Triggers
PRINT 'CREATE TRIGGER [dbo].[Dependency_Instead_Ins]';
GO
CREATE TRIGGER [dbo].[Dependency_Instead_Ins] 
   ON  [dbo].[Dependency] 
   INSTEAD OF INSERT
AS 
BEGIN
	SET NOCOUNT ON;
	INSERT INTO dbo.Dependency (PseudoCategoryID,[Table], [Column])
		OUTPUT INSERTED.*
		SELECT PseudoCategoryID, dbo.EmptyStrToNull([Table]), dbo.EmptyStrToNull([Column])
			FROM INSERTED;
END;
GO
PRINT 'CREATE TRIGGER [dbo].[Dependency_Instead_Upd]';
GO
CREATE TRIGGER [dbo].[Dependency_Instead_Upd]
   ON  [dbo].[Dependency] 
   INSTEAD OF UPDATE
AS 
BEGIN
	SET NOCOUNT ON;
	UPDATE [dbo].Dependency
		SET	PseudoCategoryID = i.PseudoCategoryID,
			[Table] = dbo.EmptyStrToNull(i.[Table]),
			[Column] = dbo.EmptyStrToNull(i.[Column])
		FROM INSERTED AS i
			WHERE Dependency.DependencyID = i.DependencyID;
END;
GO

----------------------- new Trigger on PseudoCategory -------------------------------------------------------------------------
PRINT 'CREATE TRIGGER dbo.PseudoCategory_af_ins_upd';
GO
CREATE TRIGGER dbo.PseudoCategory_af_ins_upd
   ON  [dbo].[PseudoCategory] 
   AFTER INSERT, UPDATE
AS 
BEGIN
	SET NOCOUNT ON;
	IF NOT UPDATE(Condition) RETURN;
	BEGIN TRANSACTION;
	BEGIN TRY
		DELETE FROM dbo.Cap_PseudoCategory_Int WHERE Cap_PseudoCategory_Int.PseudoCategoryID IN(SELECT PseudoCategoryID FROM INSERTED);
		DECLARE ins CURSOR FOR SELECT i.PseudoCategoryID, i.Condition FROM INSERTED i;
		OPEN ins;
		DECLARE @PseudoCategoryID INT;
		DECLARE @Condition NVARCHAR(1024);
		FETCH ins INTO @PseudoCategoryID, @Condition;
		WHILE @@FETCH_STATUS = 0 BEGIN
			DECLARE @SELECT NVARCHAR = N'CapID, ' + CAST(@PseudoCategoryID AS NVARCHAR);
			INSERT INTO dbo.Cap_PseudoCategory_Int (CapID, PseudoCategoryID)
				EXEC dbo.GetCapsOfPseudoCategory @PseudoCategoryID, @SELECT;
			FETCH ins INTO @PseudoCategoryID, @Condition;
		END; 
		CLOSE ins;
		DEALLOCATE ins;
		COMMIT;
	END TRY
	BEGIN CATCH
		ROLLBACK;
		EXEC dbo.usp_RethrowError;
		RETURN;
	END CATCH;
END;
GO
----------------------- Procedure GetPseudoCategoriesOfCap -------------------------
PRINT 'CREATE PROCEDURE [dbo].[GetPseudoCategoriesOfCap]';
GO
--- <summary>For given Cap gets all the pseudocategories it belongs to</summary>
--- <param name="CapID">ID of cap to get pseudocategories of</param>
--- <param name="SELECT">Text of SELECT clause of SELECT statement. Indicates column list to be returned from this procedure</param>
--- <returns>Result set containing all the categories of cap <paramref name="CapID"/>. Columns in result set depends on value of the <paramref name="SELECT"/> parameter</returns>
CREATE PROCEDURE [dbo].[GetPseudoCategoriesOfCap]
(
	@CapID INT,
	@SELECT NVARCHAR(MAX) = '*'
)
AS
BEGIN
	DECLARE @PseudoCategoryIDs TABLE(PseudoCategoryID INT);
	DECLARE @Cmd NVARCHAR(MAX);
	DECLARE psc CURSOR FOR SELECT PseudoCategoryID, Condition FROM dbo.PseudoCategory;
	DECLARE @PseudoCategoryID INT;
	DECLARE @Condition NVARCHAR(1024);
	OPEN psc;
	FETCH psc INTO @PseudoCategoryID, @Condition;
	WHILE @@FETCH_STATUS = 0 BEGIN
		SET @Cmd = N'SELECT CASE
			WHEN EXISTS(SELECT * FROM dbo.Cap WHERE CapID = ' + CAST(@CapID AS NVARCHAR) + N' (' + @Condition + N')) THEN
				' + CAST(@PseudoCategoryID AS NVARCHAR) + '
			ELSE NULL END';
		INSERT INTO @PseudoCategoryIDs (PseudoCategoryID)
			EXEC (@Cmd);
		FETCH psc INTO @PseudoCategoryID, @Condition;
	END;
	CLOSE psc;
	DEALLOCATE psc;
	DECLARE @IDs dbo.IntTable;
	INSERT INTO @IDs (Value) SELECT DISTINCT PseudoCategoryID FROM @PseudoCategoryIDs pci WHERE pci.PseudoCategoryID IS NOT NULL;
	SET @Cmd = 'SELECT ' + @SELECT + ' FROM dbo.PseudoCategory WHERE PseudoCategoryID IN (SELECT Value FROM @IDs)';
	EXEC sp_executesql @Cmd, N'@PseudoCategoryIDs dbo.IntTable READONLY', @IDs; 
END;
GO
------------------------------ Procedure ResetCapPseudoCategories ----------------------------------------------------
PRINT 'CREATE PROCEDURE dbo.ResetCapPseudoCategories';
GO
--- <summary>Removes all pseudocategories of given caps and creates new pseudocategories based on current cap state</summary>
--- <param name="CapID">ID of cap to reset pseudocategories associations of</param>
CREATE PROCEDURE dbo.ResetCapPseudoCategories
(@CapID INT)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION;
	BEGIN TRY
		DELETE FROM dbo.Cap_PseudoCategory_Int WHERE CapID = @CapID;
		DECLARE @SELECT NVARCHAR(1024) = CAST(@CapID AS NVARCHAR) + ', PseudoCategoryID';
		INSERT INTO dbo.Cap_PseudoCategory_Int (CapID, PseudoCategoryID)
			EXEC dbo.GetPseudoCategoriesOfCap @CapID, @SELECT;
		COMMIT;
	END TRY
	BEGIN CATCH
		ROLLBACK;
		EXEC dbo.usp_RethrowError;
		RETURN;
	END CATCH;	
END;
GO
---------------------------------- Procedure GetCapsOfPseudoCategory --------------------------------------------------
PRINT 'ALTER PROCEDURE [dbo].[GetCapsOfPseudoCategory]';
GO
--- <summary>Gets all the caps belonging to given pseudocategory</summary>
--- <param name="PseudoCategoryID">ID of pseudocategory to get caps of</param>
--- <param name="SELECT">Test of SELECT clause of SELECT command - column list of columns to be retireved from the <see cref="Cap"/> table</param>
--- <returns>Resultset containing all the caps belonging to pseudocategory <paramref name="PseudoCategoryID"/>. Columns in result set are defined by value of the <paramref name="SELECT"/> parameter</param>
ALTER PROCEDURE [dbo].[GetCapsOfPseudoCategory] 
(
	@PseudoCategoryID int,
	@SELECT NVARCHAR(MAX) = '*'
)

AS
BEGIN
	DECLARE @Condition NVARCHAR(1024);
	SET @Condition = (SELECT Condition FROM dbo.PseudoCategory pc  WHERE pc.PseudoCategoryID = @PseudoCategoryID);
	DECLARE @Cmd NVARCHAR(MAX);
	SET @Cmd = 'SELECT ' + @SELECT + ' FROM dbo.Cap WHERE ' + @Condition + '';
	EXEC (@Cmd);
END;
GO

--------------------------------------------------------------------------------------------------------------------------------
--Increase version
PRINT 'ALTER FUNCTION [dbo].[GetDatabaseVersion]';
GO
ALTER FUNCTION [dbo].[GetDatabaseVersion] ()
RETURNS nvarchar(50)
AS
BEGIN
	DECLARE @dbGuid NVARCHAR(38) = '{DAFDAE3F-2F0A-4359-81D6-50BA394D72D9}';
	DECLARE @dbVersion NVARCHAR(11) = '0.1.5.0';
	RETURN @dbGuid + @dbversion;
END;   
GO
PRINT 'Done!';
GO