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
------------------------------- Procedure AssignPseudoCategories ---------------------------------------------
PRINT 'CREATE PROCEDURE dbo.AssignPseudocategories';
GO
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
GO
----------------------------- Function IsCapInPseudoCategory -----------------------------------------------------------------------
PRINT 'CREATE FUNCTION dbo.IsCapInPseudoCategory';
GO
--- <summary>Tests if given Cap (represented by ID) belongs to given PseudoCategory (represented by condition)</summary>
--- <param name="CapID">ID of Cap (<see cref="Cap.CapID"/>) to test</param>
--- <param name="PseudoCategoryCondition">PseudoCategory condition - valid SQL code for WHERE clause</param>
--- <returns>True when <paramref name="PseudoCategoryCondition"/> is tru for Cap <paramref name="CapID"/>; false otherwise</returns>
CREATE FUNCTION dbo.IsCapInPseudoCategory
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
END;
GO
--------------------------------------- Function ColumnUpdated ------------------------------------------------------------------------------
--PRINT  'CREATE FUNCTION ColumnUpdated';
--GO
----- <summary>Gets value indicating if column with given name of given table was updated by recent command</summary>
----- <param name="Table">Name of table</param>
----- <param name="Column">Name of column in table <paramref name="Table"/></param>
----- <param name="UpdatedColumns">Value returned by <c>COLUMNS_UPDATED</c> inicating which columns were updated by recent commanmd</param>
----- <param name="TableSchema">Name of schema table belongs to</param>
----- <returns>True when column was updated by query, false otherwise</returns>
--CREATE FUNCTION dbo.ColumnUpdated
--(
--	@Table NVARCHAR(256),
--	@Column NVARCHAR(256),
--	@UpdatedColumns VARBINARY,
--	@TableSchema NVARCHAR(256) = 'dbo'
--)
--RETURNS BIT
--AS
--BEGIN
--	DECLARE @ColumnID INT = COLUMNPROPERTY(OBJECT_ID(@TableSchema + '.' + @Table), @Column, 'ColumnID');
--	DECLARE @ByteNumber INT = (@ColumnID - 1) % 8;
--	DECLARE @Byte VARBINARY	= SUBSTRING(@UpdatedColumns, @ByteNumber + 1, 1);
--	DECLARE @ColumnID_inByte INT = POWER(2, (@ColumnID - 1) % 8);
--	IF @Byte & @ColumnID_inByte <> 0 RETURN 1;
--	RETURN 0;

--END;
GO
----------------------------------- TABLE Storage ----------------------------------------------------------
EXECUTE sp_rename N'dbo.Storage.ParentStorage', N'ParentStorageID', 'COLUMN';
GO
------------------------------------ TRIGGER Storage_Instead_Ins -------------------------------------------
PRINT 'ALTER TRIGGER dbo.Storage_Instead_Ins';
GO
ALTER TRIGGER dbo.Storage_Instead_Ins 
   ON  dbo.Storage
   INSTEAD OF INSERT
AS 
BEGIN
	SET NOCOUNT ON;
    INSERT INTO dbo.Storage (StorageNumber,[Description],StorageTypeID, HasCaps, ParentStorageID)
        OUTPUT INSERTED.*
		SELECT StorageNumber, dbo.EmptyStrToNull([Description]), StorageTypeID, HasCaps, ParentStorageID
  FROM INSERTED;
END
GO
------------------------------- Trigger Storage_Instead_Upd ----------------------------------------------
PRINT 'ALTER TRIGGER [dbo].Storage_Instead_Upd';
GO
ALTER TRIGGER [dbo].Storage_Instead_Upd
    ON  [dbo].Storage 
    INSTEAD OF UPDATE
AS 
BEGIN
    SET NOCOUNT ON;
	UPDATE [dbo].Storage	
    SET	StorageNumber = i.StorageNumber,
        [Description] = dbo.EmptyStrToNull(i.[Description]),
        StorageTypeid = i.StorageTypeid,
        HasCaps = i.HasCaps,
        ParentStorageID = i.ParentStorageID
     FROM INSERTED AS i
     WHERE Storage.StorageID = i.StorageID;
END
GO
-------------------------------------------------- View CapEx -----------------------------------------------------------
PRINT 'CREATE VIEW dbo.CapEx';
GO
CREATE VIEW [dbo].[CapEx]
AS
SELECT
	c.*,
	mt.TypeName AS [MainType.TypeName], mt.[Description] AS [MainType.Description],
	ct.TypeName AS [CapType.TypeName], ct.Height AS [CapType.Height], ct.Size AS [CapType.Size], ct.Size2 AS [CapType.Size2],
		ct.MainTypeID AS [CapType.MainTypeID], ct.MaterialID AS [CapType.MaterialID], ct.ShapeID AS [CapType.ShapeID],
		ct.TargetID AS [CapType.TargetID],
	[ct.mt].TypeName AS [CapType.MainType.TypeName], [ct.mt].[Description] AS [CapType.MainType.Description],
	[ct.s].Name AS [CapType.Shape.Name], [ct.s].Size1Name AS [CapType.Shape.Size1Name], [ct.s].Size2Name AS [CapType.Shape.Size2Name],
		[ct.s].[Description] AS [CapType.Shape.Description],
	[ct.m].Name AS [CapType.Material.Name], [ct.m].[Description] AS [CapType.Material.Description],
	[ct.t].Name AS [CapType.Target.Name], [ct.t].[Description] AS [CapType.Target.Description],
	s.Name AS [Shape.Name], s.Size1Name AS [Shape.Size1Name], s.Size2Name AS [Shape.Size2Name], s.[Description] AS [Shape.Description],
	m.Name AS [Material.Name], m.[Description] AS [Material.Description],
	t.Name AS [Target.Name], t.[Description] AS [Target.Description],
	cmp.CompanyName AS [Company.CompanyName], cmp.[Description] AS [Company.Description],
	p.ProductName AS [Product.Name], p.ProductTypeID AS [Product.ProductTypeID], p.[Description] AS [Product.Description],
	pt.ProductTypeName AS [ProductType.ProductTypeName], pt.IsAlcoholic AS [ProductType.IsAlcoholic], pt.IsDrink AS [ProductType.IsDrink],
		pt.[Description] AS [ProductType.Description],
	strg.StorageNumber AS [Storage.StorageNumber], strg.HasCaps AS [Storage.HasCaps], strg.StorageTypeID AS [Storage.StorageTypeID],
		strg.ParentStorageID AS [Storage.ParentStorageID], strg.[Description] AS [Storage.Description],
	st.Name AS [Storage.StorageType.Name], st.[Description] AS [Storage.StorageType.Description],
	[strg.ps].StorageNumber AS [Storage.ParentStorage.StorageNumber], [strg.ps].HasCaps AS [Storage.ParentStorage.HasCaps],
		[strg.ps].StorageTypeID AS [Storage.ParentStorage.StorageTypeID], [strg.ps].ParentStorageID AS [Storage.ParentStorage.ParentStorageID],
		[strg.ps].[Description] AS [Storage.ParentStorage.Description]	
	FROM dbo.Cap c
		INNER JOIN dbo.MainType mt ON(c.MainTypeID = mt.MainTypeID)
		LEFT OUTER JOIN dbo.CapType ct ON(c.CapTypeID = ct.CapTypeID)
			LEFT OUTER JOIN dbo.MainType [ct.mt] ON (ct.MainTypeID = [ct.mt].MainTypeID)
			LEFT OUTER JOIN dbo.Shape [ct.s] ON (ct.ShapeID = [ct.s].ShapeID)
			LEFT OUTER JOIN dbo.Material [ct.m] ON (ct.MaterialID = [ct.m].MaterialID)
			LEFT OUTER JOIN dbo.Target [ct.t] ON (ct.TargetID = [ct.t].TargetID)
		INNER JOIN dbo.Shape s ON(c.ShapeID = s.ShapeID)
		INNER JOIN dbo.Material m ON (c.MaterialID = m.MaterialID)
		LEFT OUTER JOIN dbo.[Target] t ON(c.TargetID = t.TargetID)
		
		LEFT OUTER JOIN dbo.Company cmp ON(c.CompanyID = cmp.CompanyID)
		LEFT OUTER JOIN dbo.Product p ON(p.ProductID = c.ProductID)
		LEFT OUTER JOIN dbo.ProductType pt ON(c.ProductTypeID = pt.ProductTypeID)
		
		INNER JOIN dbo.Storage strg ON (c.StorageID = strg.StorageID)
			INNER JOIN dbo.StorageType st ON(strg.StorageTypeID = st.StorageTypeID)
			LEFT OUTER JOIN dbo.Storage [strg.ps] ON(strg.ParentStorageID = [strg.ps].StorageID);
GO
--------------------------------------------- Table Settings -----------------------------------------------------------------------
PRINT 'CREATE TABLE dbo.Settings';
GO
CREATE TABLE dbo.Settings(
	SettingID int NOT NULL IDENTITY (1, 1),
	[Key] nvarchar(255) NOT NULL,
	Value nvarchar(MAX) NULL
);
GO
ALTER TABLE dbo.Settings ADD CONSTRAINT PK_Settings PRIMARY KEY(SettingID);
GO
ALTER TABLE dbo.Settings ADD CONSTRAINT UNQ_Settings_Key UNIQUE ([Key]);
GO
ALTER TABLE dbo.Settings ADD CONSTRAINT CHK_Setting_NoEmptyKey CHECK ([Key] <> '');
GO
INSERT INTO dbo.Settings ([Key], Value)
    VALUES('Images.CapsInDatabase', NULL),
          ('Images.CapsInFileSystem', '256,64,0'),
          ('Images.CapSignStorage', 'filesystem'),
          ('Images.CapTypeStorage', 'filesystem'),
          ('Images.MainTypeStorage', 'filesystem'),
          ('Images.ShapeStorage', 'filesystem'),
          ('Images.StorageStorage', 'filesystem');
GO

------------------------------------------------- Constraints   ----------------------------------------------------------------
PRINT 'Constraints';
GO
ALTER TABLE dbo.Storage ADD CONSTRAINT CHK_Storage_NoSelfParent CHECK (StorageID <> ParentStorageID);
GO
ALTER TABLE dbo.StoredImage DROP CONSTRAINT [CHK_StoredImage_OneReference];
GO
ALTER TABLE [dbo].[StoredImage]
    ADD CONSTRAINT [CHK_StoredImage_OneReference] CHECK ((((((case when [ImageID] IS NULL then (0) else (1) end+case when [CapSignID] IS NULL then (0) else (1) end)+case when [CapTypeID] IS NULL then (0) else (1) end)+case when [MainTypeID] IS NULL then (0) else (1) end)+case when [ShapeID] IS NULL then (0) else (1) end)+case when [StorageID] IS NULL then (0) else (1) end)=(1));
GO
--==============================================================================================================================
GO
COMMIT;
BEGIN
	DECLARE @Cmd NVARCHAR(1024) =
		'ALTER DATABASE [' + DB_NAME() + '] SET CONCAT_NULL_YIELDS_NULL ON WITH NO_WAIT;
		 ALTER DATABASE [' + DB_NAME() + '] SET ANSI_PADDING ON WITH NO_WAIT;
		 ALTER DATABASE [' + DB_NAME() + '] SET ANSI_WARNINGS ON WITH NO_WAIT;
		 ALTER DATABASE [' + DB_NAME() + '] SET ANSI_NULL_DEFAULT ON WITH NO_WAIT;
		 ALTER DATABASE [' + DB_NAME() + '] SET ANSI_NULLS ON WITH NO_WAIT;
		 ALTER DATABASE [' + DB_NAME() + '] SET QUOTED_IDENTIFIER ON WITH NO_WAIT;'
	EXEC(@Cmd);
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