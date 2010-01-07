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
----------------------------------------------------------------------------------------------------------------------------------
-- CapTranslation table

CREATE TABLE [dbo].[CapTranslation](
	[CapTranslationID] [int] IDENTITY(1,1) NOT NULL,
	[CapID] [int] NOT NULL,
	[Culture] [varchar](15) NOT NULL,
	[CapName] [nvarchar](255) NULL,
	[MainPicture] [nvarchar](255) NULL,
	[Note] [nvarchar](max) NULL,
	[AnotherPictures] [nvarchar](max) NULL,
 CONSTRAINT [PK_CapTranslation] PRIMARY KEY CLUSTERED 
(
	[CapTranslationID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [UNQ_trnCap] UNIQUE NONCLUSTERED 
(
	[CapID] ASC,
	[Culture] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[CapTranslation]  WITH CHECK ADD  CONSTRAINT [FK_CapTranslation_Cap] FOREIGN KEY([CapID])
REFERENCES [dbo].[Cap] ([CapID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[CapTranslation] CHECK CONSTRAINT [FK_CapTranslation_Cap]
GO

ALTER TABLE [dbo].[CapTranslation]  WITH CHECK ADD  CONSTRAINT [CHK_CapTranslation_NoEmptyStrings] CHECK  (([Culture]<>'' AND [CapName]<>'' AND [MainPicture]<>'' AND [Note]<>'' AND [AnotherPictures]<>''))
GO

ALTER TABLE [dbo].[CapTranslation] CHECK CONSTRAINT [CHK_CapTranslation_NoEmptyStrings]
GO

--Triggers
CREATE TRIGGER [dbo].[CapTranslation_Instead_Ins] 
   ON  [dbo].CapTranslation 
   INSTEAD OF INSERT
AS 
BEGIN
	SET NOCOUNT ON;
    INSERT INTO dbo.CapTranslation
	    (CapID, Culture, CapName, MainPicture, Note, AnotherPictures)
    OUTPUT inserted.*
	SELECT CapID, dbo.EmptyStrToNull(Culture),dbo.EmptyStrToNull(CapName),dbo.EmptyStrToNull(MainPicture),dbo.EmptyStrToNull(Note),dbo.EmptyStrToNull(AnotherPictures)
    FROM inserted;
END
GO
CREATE TRIGGER [dbo].[CapTranslation_Instead_Upd]
   ON [dbo].[CapTranslation] 
   INSTEAD OF UPDATE
AS 
BEGIN
	SET NOCOUNT ON;
	UPDATE [dbo].CapTranslation
	SET	
	CapID=i.CapID,
	Culture=dbo.EmptyStrToNull(i.Culture),
	CapName=dbo.EmptyStrToNull(i.CapName),
	MainPicture=dbo.EmptyStrToNull(i.MainPicture),
	Note=dbo.EmptyStrToNull(i.Note),
	AnotherPictures=dbo.EmptyStrToNull(i.AnotherPictures)
	FROM inserted AS i
	WHERE CapTranslation.CapTranslationID=i.CapTranslationID;			 
END;
GO

-------------------------------------------------------------------------------------------------------------------------------
-- SimpleTranslation table

CREATE TABLE [dbo].[SimpleTranslation](
	[SimpleTranslationID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryID] [int] NULL,
	[KeywordID] [int] NULL,
	[ProductID] [int] NULL,
	[CompanyID] [int] NULL,
	[ProductTypeID] [int] NULL,
	[TargetID] [int] NULL,
	[MaterialID] [int] NULL,
	[CapTypeID] [int] NULL,
	[ShapeID] [int] NULL,
	[MainTypeID] [int] NULL,
	[CapSignID] [int] NULL,
	[StorageID] [int] NULL,
	[StorageTypeID] [int] NULL,
	[CapInstanceID] [int] NULL,
	[Culture] [varchar](15) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_SimpleTranslation] PRIMARY KEY CLUSTERED 
(
	[SimpleTranslationID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [UNQ_SimpleTranslation] UNIQUE NONCLUSTERED 
(
	[CategoryID] ASC,
	[KeywordID] ASC,
	[ProductID] ASC,
	[ProductTypeID] ASC,
	[TargetID] ASC,
	[MaterialID] ASC,
	[CapTypeID] ASC,
	[ShapeID] ASC,
	[MainTypeID] ASC,
	[CapSignID] ASC,
	[StorageID] ASC,
	[StorageTypeID] ASC,
	[CompanyID] ASC,
	[Culture] ASC,
	[CapInstanceID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[SimpleTranslation]  WITH CHECK ADD  CONSTRAINT [FK_SimpleTranslation_CapInstance] FOREIGN KEY([CapInstanceID])
REFERENCES [dbo].[CapInstance] ([CapInstanceID])
GO

ALTER TABLE [dbo].[SimpleTranslation] CHECK CONSTRAINT [FK_SimpleTranslation_CapInstance]
GO

ALTER TABLE [dbo].[SimpleTranslation]  WITH CHECK ADD  CONSTRAINT [FK_SimpleTranslation_CapSign] FOREIGN KEY([CapSignID])
REFERENCES [dbo].[CapSign] ([CapSignID])
GO

ALTER TABLE [dbo].[SimpleTranslation] CHECK CONSTRAINT [FK_SimpleTranslation_CapSign]
GO

ALTER TABLE [dbo].[SimpleTranslation]  WITH CHECK ADD  CONSTRAINT [FK_SimpleTranslation_CapType] FOREIGN KEY([CapTypeID])
REFERENCES [dbo].[CapType] ([CapTypeID])
GO

ALTER TABLE [dbo].[SimpleTranslation] CHECK CONSTRAINT [FK_SimpleTranslation_CapType]
GO

ALTER TABLE [dbo].[SimpleTranslation]  WITH CHECK ADD  CONSTRAINT [FK_SimpleTranslation_Category] FOREIGN KEY([CategoryID])
REFERENCES [dbo].[Category] ([CategoryID])
GO

ALTER TABLE [dbo].[SimpleTranslation] CHECK CONSTRAINT [FK_SimpleTranslation_Category]
GO

ALTER TABLE [dbo].[SimpleTranslation]  WITH CHECK ADD  CONSTRAINT [FK_SimpleTranslation_Company] FOREIGN KEY([CompanyID])
REFERENCES [dbo].[Company] ([CompanyID])
GO

ALTER TABLE [dbo].[SimpleTranslation] CHECK CONSTRAINT [FK_SimpleTranslation_Company]
GO

ALTER TABLE [dbo].[SimpleTranslation]  WITH CHECK ADD  CONSTRAINT [FK_SimpleTranslation_Keyword] FOREIGN KEY([KeywordID])
REFERENCES [dbo].[Keyword] ([KeywordID])
GO

ALTER TABLE [dbo].[SimpleTranslation] CHECK CONSTRAINT [FK_SimpleTranslation_Keyword]
GO

ALTER TABLE [dbo].[SimpleTranslation]  WITH CHECK ADD  CONSTRAINT [FK_SimpleTranslation_MainType] FOREIGN KEY([MainTypeID])
REFERENCES [dbo].[MainType] ([MainTypeID])
GO

ALTER TABLE [dbo].[SimpleTranslation] CHECK CONSTRAINT [FK_SimpleTranslation_MainType]
GO

ALTER TABLE [dbo].[SimpleTranslation]  WITH CHECK ADD  CONSTRAINT [FK_SimpleTranslation_Material] FOREIGN KEY([MaterialID])
REFERENCES [dbo].[Material] ([MaterialID])
GO

ALTER TABLE [dbo].[SimpleTranslation] CHECK CONSTRAINT [FK_SimpleTranslation_Material]
GO

ALTER TABLE [dbo].[SimpleTranslation]  WITH CHECK ADD  CONSTRAINT [FK_SimpleTranslation_Product] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Product] ([ProductID])
GO

ALTER TABLE [dbo].[SimpleTranslation] CHECK CONSTRAINT [FK_SimpleTranslation_Product]
GO

ALTER TABLE [dbo].[SimpleTranslation]  WITH CHECK ADD  CONSTRAINT [FK_SimpleTranslation_ProductType] FOREIGN KEY([ProductTypeID])
REFERENCES [dbo].[ProductType] ([ProductTypeID])
GO

ALTER TABLE [dbo].[SimpleTranslation] CHECK CONSTRAINT [FK_SimpleTranslation_ProductType]
GO

ALTER TABLE [dbo].[SimpleTranslation]  WITH CHECK ADD  CONSTRAINT [FK_SimpleTranslation_Shape] FOREIGN KEY([ShapeID])
REFERENCES [dbo].[Shape] ([ShapeID])
GO

ALTER TABLE [dbo].[SimpleTranslation] CHECK CONSTRAINT [FK_SimpleTranslation_Shape]
GO

ALTER TABLE [dbo].[SimpleTranslation]  WITH CHECK ADD  CONSTRAINT [FK_SimpleTranslation_Storage] FOREIGN KEY([StorageID])
REFERENCES [dbo].[Storage] ([StorageID])
GO

ALTER TABLE [dbo].[SimpleTranslation] CHECK CONSTRAINT [FK_SimpleTranslation_Storage]
GO

ALTER TABLE [dbo].[SimpleTranslation]  WITH CHECK ADD  CONSTRAINT [FK_SimpleTranslation_StorageType] FOREIGN KEY([StorageTypeID])
REFERENCES [dbo].[StorageType] ([StorageTypeID])
GO

ALTER TABLE [dbo].[SimpleTranslation] CHECK CONSTRAINT [FK_SimpleTranslation_StorageType]
GO

ALTER TABLE [dbo].[SimpleTranslation]  WITH CHECK ADD  CONSTRAINT [FK_SimpleTranslation_Target] FOREIGN KEY([TargetID])
REFERENCES [dbo].[Target] ([TargetID])
GO

ALTER TABLE [dbo].[SimpleTranslation] CHECK CONSTRAINT [FK_SimpleTranslation_Target]
GO

ALTER TABLE [dbo].[SimpleTranslation]  WITH CHECK ADD  CONSTRAINT [CHK_SimpleTranslation_NoEmptyStrings] CHECK  (([Culture]<>'' AND [Name]<>'' AND [Description]<>''))
GO

ALTER TABLE [dbo].[SimpleTranslation] CHECK CONSTRAINT [CHK_SimpleTranslation_NoEmptyStrings]
GO

ALTER TABLE [dbo].[SimpleTranslation]  WITH CHECK ADD  CONSTRAINT [CHK_SimpleTranslation_OnlyOneParent] CHECK  (((((((((((((((case when [CategoryID] IS NULL then (0) else (1) end+case when [KeywordID] IS NULL then (0) else (1) end)+case when [ProductID] IS NULL then (0) else (1) end)+case when [CompanyID] IS NULL then (0) else (1) end)+case when [ProductTypeID] IS NULL then (0) else (1) end)+case when [TargetID] IS NULL then (0) else (1) end)+case when [MaterialID] IS NULL then (0) else (1) end)+case when [CapTypeID] IS NULL then (0) else (1) end)+case when [ShapeID] IS NULL then (0) else (1) end)+case when [MainTypeID] IS NULL then (0) else (1) end)+case when [CapSignID] IS NULL then (0) else (1) end)+case when [StorageID] IS NULL then (0) else (1) end)+case when [StorageTypeID] IS NULL then (0) else (1) end)+case when [CapInstanceID] IS NULL then (0) else (1) end)=(1)))
GO

ALTER TABLE [dbo].[SimpleTranslation] CHECK CONSTRAINT [CHK_SimpleTranslation_OnlyOneParent]
GO 
--Triggers
CREATE TRIGGER [dbo].[SimpleTranslation_Instead_Ins] 
   ON  [dbo].SimpleTranslation 
   INSTEAD OF INSERT
AS 
BEGIN
	SET NOCOUNT ON;
	INSERT INTO dbo.SimpleTranslation
		(CategoryID, KeywordID, ProductID, CompanyID, ProductTypeID, TargetID, MaterialID, CapTypeID, ShapeID, MainTypeID, CapSignID, StorageID, StorageTypeID, CapInstanceID, Culture, Name, [Description])
        OUTPUT inserted.*
		SELECT CategoryID, KeywordID, ProductID, CompanyID, ProductTypeID, TargetID, MaterialID, CapTypeID, ShapeID, MainTypeID, CapSignID, StorageID, StorageTypeID, CapInstanceID, dbo.EmptyStrToNull(Culture), dbo.EmptyStrToNull(Name), dbo.EmptyStrToNull([Description])
	FROM inserted;
END
GO
CREATE TRIGGER [dbo].[SimpleTranslation_Instead_Upd]
   ON [dbo].[SimpleTranslation] 
   INSTEAD OF UPDATE
AS 
BEGIN
	SET NOCOUNT ON;
	UPDATE [dbo].SimpleTranslation
	SET	
		CategoryID    = i.CategoryID,
		KeywordID     = i.KeywordID,
		ProductID     = i.ProductID,
		CompanyID     = i.CompanyID,
		ProductTypeID = i.ProductTypeID,
		TargetID      = i.TargetID,
		MaterialID    = i.MaterialID,
		CapTypeID     = i.CapTypeID,
		ShapeID       = i.ShapeID,
		MainTypeID    = i.MainTypeID,
		CapSignID     = i.CapSignID,
		StorageID     = i.StorageID,
		StorageTypeID = i.StorageTypeID,
		CapInstanceID = i.CapInstanceID,
		Culture       = dbo.EmptyStrToNull(i.Culture),
		Name          = dbo.EmptyStrToNull(i.Name),
		[Description] = dbo.EmptyStrToNull(i.[Description])
	FROM inserted AS i
	WHERE SimpleTranslation.SimpleTranslationID=i.SimpleTranslationID;			 
END;
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