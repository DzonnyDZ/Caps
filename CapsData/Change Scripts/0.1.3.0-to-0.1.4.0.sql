--Table Storage

BEGIN TRANSACTION;
GO
ALTER TABLE dbo.Storage ADD
	ParentStorage int NULL,
	HasCaps bit NOT NULL CONSTRAINT DF_Storage_HasCaps DEFAULT 1;
GO
ALTER TABLE dbo.Storage ADD CONSTRAINT
	FK_Storage_ParentStorage FOREIGN KEY
	(
	ParentStorage
	) REFERENCES dbo.Storage
	(
	StorageID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION;
GO
COMMIT;
GO
-------------------------------------------------------------------------------------------------------------------------------
-- Table StoredImage
BEGIN TRANSACTION;
GO
CREATE TABLE dbo.StoredImage
	(
	StoredImageID int NOT NULL IDENTITY (1, 1),
	FileName nvarchar(1024) NOT NULL,
	MIME nvarchar(100) NOT NULL,
	Size int NOT NULL,
	Width int NOT NULL,
	Height int NOT NULL,
	ImageID int NULL,
	CapSignID int NULL,
	CapTypeID int NULL,
	MainTypeID int NULL,
	ShapeID int NULL,
	StorageID int NULL,
	Data varbinary(MAX) NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY];
GO
ALTER TABLE dbo.StoredImage ADD CONSTRAINT
	CHK_StoredImage_AtLeastOneReference CHECK (CASE WHEN
		ImageID IS NOT NULL OR
		CapSignID IS NOT NULL OR
		CapTypeID IS NOT NULL OR
		MainTypeID IS NOT NULL OR
		ShapeID IS NOT NULL OR
		StorageID IS NOT NULL
		THEN 1 ELSE 0 END = 1
		);
GO
ALTER TABLE dbo.StoredImage ADD CONSTRAINT
	CHK_StoredImage_OnlyOneReference CHECK (
	CASE WHEN
	(ImageID IS NOT NULL AND CapSignID IS NULL AND CapTypeID IS NULL AND MainTypeID IS NULL AND ShapeID IS NULL AND StorageID IS NULL) OR
	(ImageID IS NULL AND CapSignID IS NOT NULL AND CapTypeID IS NULL AND MainTypeID IS NULL AND ShapeID IS NULL AND StorageID IS NULL) OR
	(ImageID IS NULL AND CapSignID IS NULL AND CapTypeID IS NOT NULL AND MainTypeID IS NULL AND ShapeID IS NULL AND StorageID IS NULL) OR
	(ImageID IS NULL AND CapSignID IS NULL AND CapTypeID IS NULL AND MainTypeID IS NOT NULL AND ShapeID IS NULL AND StorageID IS NULL) OR
	(ImageID IS NULL AND CapSignID IS NULL AND CapTypeID IS NULL AND MainTypeID IS NULL AND ShapeID IS NOT NULL AND StorageID IS NULL) OR
	(ImageID IS NULL AND CapSignID IS NULL AND CapTypeID IS NULL AND MainTypeID IS NULL AND ShapeID IS NULL AND StorageID IS NOT NULL) 
	THEN 1 ELSE 0 END = 1
	)
GO
ALTER TABLE dbo.StoredImage ADD CONSTRAINT
	PK_StoredImage PRIMARY KEY CLUSTERED 
	(
	StoredImageID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.StoredImage ADD CONSTRAINT
	FK_StoredImage_Image FOREIGN KEY
	(
	ImageID
	) REFERENCES dbo.Image
	(
	ImageID
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE dbo.StoredImage ADD CONSTRAINT
	FK_StoredImage_CapSign FOREIGN KEY
	(
	CapSignID
	) REFERENCES dbo.CapSign
	(
	CapSignID
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE dbo.StoredImage ADD CONSTRAINT
	FK_StoredImage_CapType FOREIGN KEY
	(
	CapTypeID
	) REFERENCES dbo.CapType
	(
	CapTypeID
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE dbo.StoredImage ADD CONSTRAINT
	FK_StoredImage_MainType FOREIGN KEY
	(
	MainTypeID
	) REFERENCES dbo.MainType
	(
	MainTypeID
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE dbo.StoredImage ADD CONSTRAINT
	FK_StoredImage_Shape FOREIGN KEY
	(
	ShapeID
	) REFERENCES dbo.Shape
	(
	ShapeID
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE dbo.StoredImage ADD CONSTRAINT
	FK_StoredImage_Storage FOREIGN KEY
	(
	StorageID
	) REFERENCES dbo.Storage
	(
	StorageID
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE dbo.StoredImage ADD CONSTRAINT CHK_StoredImage_NoEmptyStrings CHECK (FileName <> '' AND MIME <> '');
GO
COMMIT;
GO

--------------------------------------------------------------------------------------------------------------------------------------
--Multiple signs per cap

BEGIN TRANSACTION
GO
CREATE TABLE dbo.Cap_CapSign_Int
	(
	CapID int NOT NULL,
	CapSignID int NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Cap_CapSign_Int ADD CONSTRAINT
	PK_Cap_CapSign_Int PRIMARY KEY CLUSTERED 
	(
	CapID,
	CapSignID
	)  ON [PRIMARY]

GO
ALTER TABLE dbo.Cap_CapSign_Int ADD CONSTRAINT
	FK_Cap_CapSign_Int_Cap FOREIGN KEY
	(
	CapID
	) REFERENCES dbo.Cap
	(
	CapID
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
ALTER TABLE dbo.Cap_CapSign_Int ADD CONSTRAINT
	FK_Cap_CapSign_Int_CapSign FOREIGN KEY
	(
	CapSignID
	) REFERENCES dbo.CapSign
	(
	CapSignID
	) ON UPDATE  NO ACTION 
	 ON DELETE  CASCADE 
	
GO
COMMIT
GO

BEGIN TRANSACTION;

insert into dbo.Cap_CapSign_int (CapID,CapSignID)
SELECT c.CapID,c.CapSignID
FROM dbo.Cap c
WHERE c.CapSignID IS NOT NULL;
GO
ALTER TABLE dbo.Cap	DROP CONSTRAINT FK_Cap_CapSign;
GO
ALTER TABLE dbo.Cap	DROP COLUMN CapSignID;
GO

COMMIT;
GO
-------------------------------------------------------------------------------------------------------------------------
--Pseudocategory
BEGIN TRANSACTION;
GO
CREATE TABLE dbo.PseudoCategory
	(
	PseudoCategoryID int NOT NULL IDENTITY (1, 1),
	Name nvarchar(50) NOT NULL,
	Description nvarchar(MAX) NULL,
	Condition nvarchar(1024) NOT NULL
	)  ON [PRIMARY]
	 TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE dbo.PseudoCategory ADD CONSTRAINT
	PK_PseudoCategory PRIMARY KEY CLUSTERED 
	(
	PseudoCategoryID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.PseudoCategory ADD CONSTRAINT
	CHK_PseudoCategory_NoEmptyStrings CHECK (Name <> '' AND Description <> '' AND Condition <> '');
GO	
COMMIT;
GO
CREATE TRIGGER [dbo].[PseudoCategory_Instead_Ins] 
   ON  [dbo].[PseudoCategory]
   instead of INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
			 insert into dbo.PseudoCategory
					 (	 Name,[Description],Condition)
         output inserted.*
			 SELECT dbo.EmptyStrToNull(Name),dbo.EmptyStrToNull([Description]),	dbo.EmptyStrToNull(Condition)
  FROM inserted	 ;


     -- select * from dbo.shape where shapeid=scope_identity();
			 
END
GO

CREATE TRIGGER [dbo].[PseudoCategory_Instead_Upd]
   ON  [dbo].PseudoCategory 
   instead of update
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	  UPDATE [dbo].PseudoCategory	
   SET	
   name=dbo.EmptyStrToNull(i.name)   ,
      [Description]=dbo.EmptyStrToNull(i.[description]),
      Condition=dbo.EmptyStrToNull(i.Condition)
     

 from inserted	as i
 WHERE pseudocategory.pseudocategoryid=i.pseudocategoryid

					;
			 
END
GO

--------------------------------------------------------------------------------------------------------------------------------
--Increase version
ALTER FUNCTION [dbo].[GetDatabaseVersion] ()
RETURNS nvarchar(50)
AS
BEGIN
declare @dbGuid nvarchar(38) = '{DAFDAE3F-2F0A-4359-81D6-50BA394D72D9}';
declare @dbVersion nvarchar(11) = '0.1.4.0';
return @dbGuid + @dbversion;

END;   
GO