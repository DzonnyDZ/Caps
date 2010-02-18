BEGIN TRANSACTION;
--Table Storage
PRINT 'ALTER TABLE dbo.Storage';
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
-------------------------------------------------------------------------------------------------------------------------------
-- Table StoredImage
PRINT 'CREATE TABLE dbo.StoredImage';
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
ALTER TABLE [dbo].[StoredImage]  WITH CHECK ADD  CONSTRAINT [CHK_StoredImage_OneReference] CHECK  (((((((case when [ImageID] IS NULL then (1) else (0) end+case when [CapSignID] IS NULL then (1) else (0) end)+case when [CapTypeID] IS NULL then (1) else (0) end)+case when [MainTypeID] IS NULL then (1) else (0) end)+case when [ShapeID] IS NULL then (1) else (0) end)+case when [StorageID] IS NULL then (1) else (0) end)=(1)))
GO
ALTER TABLE [dbo].[StoredImage] CHECK CONSTRAINT [CHK_StoredImage_OneReference]
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

--------------------------------------------------------------------------------------------------------------------------------------
--Multiple signs per cap
PRINT 'CREATE TABLE dbo.Cap_CapSign_Int';
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
PRINT 'INSERT INTO dbo.Cap_CapSign_int';

INSERT INTO dbo.Cap_CapSign_int (CapID,CapSignID)
SELECT c.CapID,c.CapSignID
FROM dbo.Cap c
WHERE c.CapSignID IS NOT NULL;
GO
ALTER TABLE dbo.Cap	DROP CONSTRAINT FK_Cap_CapSign;
GO
ALTER TABLE dbo.Cap	DROP COLUMN CapSignID;
GO
-------------------------------------------------------------------------------------------------------------------------
--Pseudocategory
PRINT 'CREATE TABLE dbo.PseudoCategory';
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
CREATE TRIGGER [dbo].[PseudoCategory_Instead_Ins] 
   ON  [dbo].[PseudoCategory]
   INSTEAD OF INSERT
AS 
BEGIN
	SET NOCOUNT ON;
    INSERT INTO dbo.PseudoCategory (Name,[Description],Condition)
        OUTPUT inserted.*
		SELECT dbo.EmptyStrToNull(Name),dbo.EmptyStrToNull([Description]), dbo.EmptyStrToNull(Condition)
			FROM INSERTED;
END
GO

CREATE TRIGGER [dbo].[PseudoCategory_Instead_Upd]
   ON  [dbo].PseudoCategory 
   INSTEAD OF UPDATE
AS 
BEGIN
	SET NOCOUNT ON;
	UPDATE [dbo].PseudoCategory	
	SET	
		Name = dbo.EmptyStrToNull(i.Name),
		[Description] = dbo.EmptyStrToNull(i.[Description]),
		Condition = dbo.EmptyStrToNull(i.Condition)
	FROM INSERTED as i
	WHERE PseudoCategory.PseudoCategoryID = i.PseudoCategoryID;
END
GO
----------------------------------------------------------------------------------------------------------------------------------
-- CapTranslation table
PRINT 'CREATE TABLE [dbo].[CapTranslation]';
GO
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
PRINT 'CREATE TABLE [dbo].[SimpleTranslation]';
GO
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
	[MainTypeID] ASC,
	[CapSignID] ASC,
	[StorageID] ASC,
	[StorageTypeID] ASC,
	[CompanyID] ASC,
	[CapInstanceID] ASC,
	[Culture] ASC
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

ALTER TABLE [dbo].[SimpleTranslation]  WITH CHECK ADD  CONSTRAINT [CHK_SimpleTranslation_OnlyOneParent] CHECK  ((((((((((((((case when [CategoryID] IS NULL then (0) else (1) end+case when [KeywordID] IS NULL then (0) else (1) end)+case when [ProductID] IS NULL then (0) else (1) end)+case when [CompanyID] IS NULL then (0) else (1) end)+case when [ProductTypeID] IS NULL then (0) else (1) end)+case when [TargetID] IS NULL then (0) else (1) end)+case when [MaterialID] IS NULL then (0) else (1) end)+case when [CapTypeID] IS NULL then (0) else (1) end)+case when [MainTypeID] IS NULL then (0) else (1) end)+case when [CapSignID] IS NULL then (0) else (1) end)+case when [StorageID] IS NULL then (0) else (1) end)+case when [StorageTypeID] IS NULL then (0) else (1) end)+case when [CapInstanceID] IS NULL then (0) else (1) end)=(1)))
GO

ALTER TABLE [dbo].[SimpleTranslation] CHECK CONSTRAINT [CHK_SimpleTranslation_OnlyOneParent]
GO


--Triggers
GO
CREATE TRIGGER [dbo].[SimpleTranslation_Instead_Ins] 
   ON  [dbo].[SimpleTranslation] 
   INSTEAD OF INSERT
AS 
BEGIN
	SET NOCOUNT ON;
	INSERT INTO dbo.SimpleTranslation
		(CategoryID, KeywordID, ProductID, CompanyID, ProductTypeID, TargetID, MaterialID, CapTypeID,  MainTypeID, CapSignID, StorageID, StorageTypeID, CapInstanceID, Culture, Name, [Description])
        OUTPUT inserted.*
		SELECT CategoryID, KeywordID, ProductID, CompanyID, ProductTypeID, TargetID, MaterialID, CapTypeID,  MainTypeID, CapSignID, StorageID, StorageTypeID, CapInstanceID, dbo.EmptyStrToNull(Culture), dbo.EmptyStrToNull(Name), dbo.EmptyStrToNull([Description])
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
-----------------------------------------------------------------------------------------------------------------------------
--ShapeTranslation table
PRINT 'CREATE TABLE [dbo].[ShapeTranslation]';
GO
CREATE TABLE [dbo].[ShapeTranslation](
	[ShapeTranslationID] [int] IDENTITY(1,1) NOT NULL,
	[ShapeID] [int] NOT NULL,
	[Culture] [varchar](15) NOT NULL,
	[Name] [nvarchar](50) NULL,
	[Size1Name] [nvarchar](50) NULL,
	[Size2Name] [nvarchar](50) NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_ShapeTranslation] PRIMARY KEY CLUSTERED 
(
	[ShapeTranslationID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [UNQ_ShapeTranslation] UNIQUE NONCLUSTERED 
(
	[ShapeID] ASC,
	[Culture] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[ShapeTranslation]  WITH CHECK ADD  CONSTRAINT [FK_ShapeTranslation_Shape] FOREIGN KEY([ShapeID])
REFERENCES [dbo].[Shape] ([ShapeID])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ShapeTranslation] CHECK CONSTRAINT [FK_ShapeTranslation_Shape]
GO

ALTER TABLE [dbo].[ShapeTranslation]  WITH CHECK ADD  CONSTRAINT [CHK_ShapeTranslation_NoEmptyStrings] CHECK  (([Culture]<>'' AND [Size1Name]<>'' AND [Size2Name]<>'' AND [Description]<>''))
GO

ALTER TABLE [dbo].[ShapeTranslation] CHECK CONSTRAINT [CHK_ShapeTranslation_NoEmptyStrings]
GO
--Triggers
GO
CREATE TRIGGER [dbo].[ShapeTranslation_Instead_Ins] 
   ON  [dbo].[ShapeTranslation] 
   INSTEAD OF INSERT
AS 
BEGIN
	SET NOCOUNT ON;
	INSERT INTO dbo.ShapeTranslation
		(ShapeID,Culture,Name,Size1Name,Size2Name,[Description])
        OUTPUT inserted.*
		SELECT ShapeID,  dbo.EmptyStrToNull(Culture), dbo.EmptyStrToNull(Name),dbo.EmptyStrToNull(Size1Name),dbo.EmptyStrToNull(Size2Name), dbo.EmptyStrToNull([Description])
	FROM inserted;
END
GO
CREATE TRIGGER [dbo].[ShapeTranslation_Instead_Upd]
   ON [dbo].[ShapeTranslation] 
   INSTEAD OF UPDATE
AS 
BEGIN
	SET NOCOUNT ON;
	UPDATE [dbo].ShapeTranslation
	SET	
		ShapeID    = i.ShapeID,
		Culture       = dbo.EmptyStrToNull(i.Culture),
		Name          = dbo.EmptyStrToNull(i.Name),
		[Description] = dbo.EmptyStrToNull(i.[Description])	,
		Size1Name          = dbo.EmptyStrToNull(i.Size1Name),
		Size2Name = dbo.EmptyStrToNull(i.Size2Name)
	FROM inserted AS i
	WHERE ShapeTranslation.ShapeTranslationID = i.ShapeTranslationID;			 
END;

GO

------------------------------------------------------------------------------------------------------------------------------
--Romanization table
PRINT 'CREATE TABLE [dbo].[Romanization]';
GO
CREATE TABLE [dbo].[Romanization](
	[Character] [nchar](1) COLLATE Latin1_General_BIN2 NOT NULL,
	[Romanization] [nvarchar](10) NOT NULL,
	[Code]  AS (unicode([Character])) PERSISTED,
 CONSTRAINT [PK_Romanization] PRIMARY KEY CLUSTERED 
(
	[Character] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

--Data
PRINT 'INSERT INTO Romanization';
INSERT INTO Romanization VALUES (N'А',N'A'), (N'а',N'a'),
(N'Б',N'B'), (N'б',N'b'),
(N'В',N'V'), (N'в',N'v'),
(N'Г',N'G'), (N'г',N'g'),
(N'Ґ',N'G̀'), (N'ґ',N'g̀'),
(N'Д',N'D'), (N'д',N'd'),
(N'Ѓ',N'Ǵ'), (N'ѓ',N'ǵ'),
(N'Ђ',N'Đ'), (N'ђ',N'đ'),
(N'Е',N'E'), (N'е',N'e'),
(N'Ё',N'Ë'), (N'ё',N'ë'),
(N'Є',N'Ê'), (N'є',N'ê'),
(N'Ж',N'Ž'), (N'ж',N'ž'),
(N'з',N'Z'), (N'З',N'z'),
(N'Ѕ',N'Ẑ'), (N'ѕ',N'Ẑ'),
(N'И',N'I'), (N'и',N'i'),
(N'І',N'Ì'), (N'і',N'ì'),
(N'Ї',N'Ï'), (N'ї',N'ï'),
(N'Ј',N'ǰ'), (N'ј',N'ǰ'),
(N'к',N'k'), (N'К',N'k'),
(N'л',N'L'), (N'Л',N'l'),
(N'Љ',N'L̂'), (N'љ',N'l̂'),
(N'М',N'M'), (N'м',N'm'),
(N'Н',N'N'), (N'н',N'n'),
(N'Њ',N'N̂'), (N'њ',N'n̂'),
(N'О',N'O'), (N'о',N'o'),
(N'Р',N'R'), (N'р',N'r'),
(N'С',N'S'), (N'с',N's'),
(N'Т',N'T'), (N'т',N't'),
(N'Ќ',N'Ḱ'), (N'ќ',N'Ḱ'),
(N'Ћ',N'Ć'), (N'ћ',N'ć'),
(N'У',N'U'), (N'у',N'u'),
(N'Ў',N'Ŭ'), (N'ў',N'ŭ'),
(N'Ф',N'F'), (N'ф',N'f'),
(N'Х',N'H'), (N'х',N'h'),
(N'Ц',N'C'), (N'ц',N'c'),
(N'Ч',N'Č'), (N'ч',N'č'),
(N'Џ',N'D̂'), (N'џ',N'd̂'),
(N'Ш',N'Š'), (N'ш',N'š'),
(N'Щ',N'Ŝ'), (N'щ',N'ŝ'),
(N'Ъ',N'ʺ'), (N'ъ',N'ʺ'),
(N'Ы',N'Y'), (N'ы',N'y'),
(N'Ь',N''''), (N'ь',N''''),
(N'Ѣ',N'Ě'), (N'ѣ',N'ě'),
(N'Э',N'È'), (N'э',N'è'),
(N'Ю',N'Û'), (N'ю',N'û'),
(N'Я',N'Â'), (N'я',N'â'),
(N'Ѡ',N'Ô'), (N'ѡ',N'ô'),
(N'Ѧ',N'Ę'), (N'ѧ',N'ę'),
(N'Ѩ',N'Ḝ'), (N'ѩ',N'ḝ'),
(N'Ѫ',N'Ǫ'), (N'ѫ',N'ǫ'),
(N'Ѭ',N'Ộ'), (N'ѭ',N'ộ'),
(N'Ѯ',N'X'), (N'ѯ',N'x'),
(N'Ѱ',N'PS'), (N'ѱ',N'ps'),
(N'Ѳ',N'F̀'), (N'ѳ',N'f̀'),
(N'Ѵ',N'Ỳ'), (N'ѵ',N'ỳ'),
(N'Ѥ',N'JE'), (N'ѥ',N'je'),
(N'Ѹ',N'OU'), (N'ѹ',N'ou'),
(N'Ѻ',N'O'), (N'ѻ',N'o'),
(N'Ѽ',N'Ỗ'), (N'ѽ',N'ỗ'),
(N'Ѿ',N'OT'), (N'ѿ',N'ot'),
(N'Ҁ',N'Q'), (N'ҁ',N'q'),
(N'Ғ',N'Ḡ'), (N'ғ',N'ḡ'),
(N'Ҕ',N'Ḡ'), (N'ҕ',N'ḡ'),
(N'Җ',N'Ẑ'), (N'җ',N'ẑ'),
(N'Ҙ',N'Ð'), (N'ҙ',N'ð'),
(N'Қ',N'K'), (N'қ',N'k'),
(N'Ҝ',N'K'), (N'ҝ',N'k'),
(N'Ҟ',N'K'), (N'ҟ',N'k'),
(N'Ҡ',N'K'), (N'ҡ',N'k'),
(N'Ң',N'N'), (N'ң',N'n'),
(N'Ҥ',N'N'), (N'ҥ',N'n'),
(N'Ҧ',N'Ṗ'), (N'ҧ',N'ṗ'),
(N'Ҩ',N'Ọ'), (N'ҩ',N'ọ'),
(N'Ҫ',N'Ş'), (N'ҫ',N'ş'),
(N'Ҭ',N'T'), (N'ҭ',N't'),
(N'Ү',N'U'), (N'ү',N'u'),
(N'Ұ',N'U'), (N'ұ',N'u'),
(N'Ҳ',N'Ħ'), (N'ҳ',N'ħ'),
(N'Ҵ',N'C̄'), (N'ҵ',N'c̄'),
(N'Ҷ',N'Ç'), (N'ҷ',N'ç'),
(N'Ҹ',N'Ç'), (N'ҹ',N'ç'),
(N'Һ',N'H'), (N'һ',N'h'),
(N'Ҽ',N'E'), (N'ҽ',N'e'),
(N'Ҿ',N'E'), (N'ҿ',N'e'),
(N'Ӂ',N'Đ'), (N'ӂ',N'đ'),
(N'Ӄ',N'K'), (N'ӄ',N'k'),
(N'Ӈ',N'Ƞ'), (N'ӈ',N'ƞ'),
(N'Ӌ',N'Č'), (N'ӌ',N'č'),
(N'Ӑ',N'Ă'), (N'ӑ',N'ă'),
(N'Ӓ',N'Ä'), (N'ӓ',N'ä'),
(N'Ӕ',N'Æ'), (N'ӕ',N'æ'),
(N'Ӗ',N'Ĕ'), (N'ӗ',N'ĕ'),
(N'Ә',N'Ə'), (N'ә',N'ə'),
(N'Ӛ',N'Ӛ'), (N'ӛ',N'ӛ'),
(N'Ӝ',N'Ž'), (N'ӝ',N'ž'),
(N'Ӟ',N'Z'), (N'ӟ',N'z'),
(N'Ӡ',N'DZ'), (N'ӡ',N'dz'),
(N'Ӣ',N'Ī'), (N'ӣ',N'ī'),
(N'Ӥ',N'Ï'), (N'ӥ',N'ï'),
(N'Ӧ',N'Ö'), (N'ӧ',N'ö'),
(N'Ө',N'Ô'), (N'ө',N'ô'),
(N'Ӫ',N'Ø'), (N'ӫ',N'ø'),
(N'Ӯ',N'Ū'), (N'ӯ',N'ū'),
(N'Ӱ',N'Ü'), (N'ӱ',N'ü'),
(N'Ӳ',N'Ű'), (N'ӳ',N'ű'),
(N'Ӵ',N'Č'), (N'ӵ',N'č'),
(N'Ӹ',N'Ÿ'), (N'ӹ',N'ÿ'),
(N'Ӏ',N'I'),
 (N'Ԟ',N'K'),
  (N'Ԉ',N'Ľ'),
(N'ԉ',N'ľ'),
(N'Ӎ',N'Ṃ'), (N'ӎ',N'ṃ'),
(N'Ӊ',N'Ṇ'), (N'ӊ',N'ṇ'),
(N'Ҏ',N'Ṛ'), (N'ҏ',N'ṛ'),
(N'Ѷ',N'Ỳ'), (N'ѷ',N'ỳ'),
(N'Α',N'A'), (N'α',N'a'),
(N'Β',N'B'), (N'β',N'b'),
(N'Γ',N'G'), (N'γ',N'g'),
(N'Δ',N'D'), (N'δ',N'd'),
(N'Ε',N'E'), (N'ε',N'e'),
(N'Ζ',N'Z'), (N'ζ',N'z'),
(N'Η',N'E'), (N'η',N'e'),
(N'Θ',N'TH'), (N'θ',N'th'),
(N'Ι',N'I'), (N'ι',N'i'),
(N'Κ',N'K'), (N'κ',N'k'),
(N'Λ',N'L'), (N'λ',N'l'),
(N'Μ',N'M'), (N'μ',N'm'),
(N'Ν',N'N'), (N'ν',N'n'),
(N'Ξ',N'X'), (N'ξ',N'x'),
(N'Ο',N'O'), (N'ο',N'o'),
(N'Π',N'P'), (N'π',N'p'),
(N'Ρ',N'R'), (N'ρ',N'r'),
(N'Σ',N'S'), (N'σ',N's'),
 (N'ς',N's'),
(N'Τ',N'T'), (N'τ',N't'),
(N'Υ',N'Y'), (N'υ',N'y'),
(N'Χ',N'CH'), (N'χ',N'ch'),
(N'Ψ',N'PS'), (N'ψ',N'ps'),
(N'Ω',N'Ó'), (N'ω',N'ó'),
(N'Ϝ',N'W'), (N'ϝ',N'w'),
(N'Ϙ',N'Q'), (N'ϙ',N'q'),
(N'Ϟ',N'Q'), (N'ϟ',N'q'),
(N'Ϻ',N'Ś'), (N'ϻ',N'ś'),
(N'Ϡ',N'ß'), (N'ϡ',N'ß'),
(N'Ϛ',N'ST'), (N'ϛ',N'st'),
(N'Ϸ',N'Þ'), (N'ϸ',N'þ'),
(N'Ά',N'Á'), (N'ά',N'á'),
(N'Έ',N'É'), (N'έ',N'é'),
(N'Ή',N'Í'), (N'ή',N'í'),
(N'Ί',N'Í'), (N'ί',N'í'),
(N'Ό',N'Ó'), (N'ό',N'ó'),
(N'Ύ',N'Ý'), (N'ύ',N'ý'),
(N'Ώ',N'Ó'), (N'ώ',N'ó'),
(N'Ϊ',N'Ï'), (N'ϊ',N'ï'),
(N'Ϋ',N'Ÿ'), (N'ϋ',N'ÿ'),
 (N'ΐ',N'ḯ'),
  (N'ΰ',N'ÿ'),
(N'Ἀ',N'A'), (N'ἀ',N'a'),
(N'Ἁ',N'A'), (N'ἁ',N'a'),
(N'Ἂ',N'A'), (N'ἂ',N'a'),
(N'Ἃ',N'A'), (N'ἃ',N'a'),
(N'Ἄ',N'A'), (N'ἄ',N'a'),
(N'Ἆ',N'A'), (N'ἆ',N'a'),
(N'Ἇ',N'A'), (N'ἇ',N'a'),
 (N'ἐ',N'e'),
  (N'ἑ',N'e'),
(N'Ἑ',N'E'),
(N'Ἒ',N'E'), (N'ἒ',N'e'),
(N'Ἓ',N'E'), (N'ἓ',N'e'),
(N'Ἔ',N'E'), (N'ἔ',N'e'),
(N'Ἠ',N'I'), (N'ἠ',N'i'),
(N'Ἡ',N'I'), (N'ἡ',N'i'),
(N'Ἢ',N'I'), (N'ἢ',N'i'),
(N'Ἣ',N'I'), (N'ἣ',N'i'),
(N'Ἤ',N'I'), (N'ἤ',N'i'),
(N'Ἦ',N'I'), (N'ἦ',N'i'),
(N'Ἧ',N'I'), (N'ἧ',N'i'),
(N'Ἰ',N'I'), (N'ἰ',N'i'),
(N'Ἱ',N'I'), (N'ἱ',N'i'),
(N'Ἲ',N'I'), (N'ἲ',N'i'),
(N'Ἳ',N'I'), (N'ἳ',N'i'),
(N'Ἴ',N'I'), (N'ἴ',N'i'),
(N'Ἶ',N'I'), (N'ἶ',N'i'),
(N'Ἷ',N'I'), (N'ἷ',N'i'),
(N'Ὀ',N'O'), (N'ὀ',N'o'),
(N'Ὁ',N'O'), (N'ὁ',N'o'),
(N'Ὂ',N'O'), (N'ὂ',N'o'),
(N'Ὃ',N'O'), (N'ὃ',N'o'),
(N'Ὄ',N'O'), (N'ὄ',N'o'),
(N'Ὑ',N'Y'), (N'ὐ',N'y'),
(N'Ὓ',N'Y'), (N'ὑ',N'y'),
(N'Ὕ',N'Y'), (N'ὒ',N'y'),
(N'Ὗ',N'Y'), (N'ὓ',N'y'),
(N'ὔ',N'y'),
(N'ὗ',N'Y'), (N'ὖ',N'y'),
(N'Ὠ',N'Ó'), (N'ὠ',N'ó'),
(N'Ὡ',N'Ó'), (N'ὡ',N'ó'),
(N'Ὢ',N'Ó'), (N'ὢ',N'ó'),
(N'Ὣ',N'Ó'), (N'ὣ',N'ó'),
(N'Ὤ',N'Ó'), (N'ὤ',N'ó'),
(N'Ὧ',N'Ó'), (N'ὧ',N'ó'),
 (N'ὰ',N'à'),
  (N'ὲ',N'è'),
   (N'ὴ',N'ì'),
   (N'ὶ',N'ì'),
    (N'ὸ',N'ò'),
    (N'ὺ',N'ỳ'),
     (N'ὼ',N'ó'),
(N'ᾈ',N'A'), (N'ᾀ',N'a'),
(N'ᾉ',N'A'), (N'ᾁ',N'a'),
(N'ᾊ',N'A'), (N'ᾂ',N'a'),
(N'ᾋ',N'A'), (N'ᾃ',N'a'),
(N'ᾌ',N'A'), (N'ᾄ',N'a'),
(N'ᾎ',N'A'), (N'ᾆ',N'a'),
(N'ᾘ',N'I'), (N'ᾐ',N'i'),
(N'ᾙ',N'I'), (N'ᾑ',N'i'),
(N'ᾚ',N'I'), (N'ᾒ',N'i'),
(N'ᾛ',N'I'), (N'ᾓ',N'i'),
(N'ᾜ',N'I'), (N'ᾔ',N'i'),
(N'ᾞ',N'I'),
(N'ᾖ',N'i'),
(N'ᾨ',N'Ó'), (N'ᾠ',N'ó'),
(N'ᾩ',N'Ó'), (N'ᾡ',N'ó'),
(N'ᾪ',N'Ó'), (N'ᾢ',N'ó'),
(N'ᾫ',N'Ó'), (N'ᾣ',N'ó'),
(N'ᾬ',N'Ó'), (N'ᾤ',N'ó'),
(N'ᾮ',N'Ó'), (N'ᾦ',N'ó'),
(N'Ᾰ',N'Ă'), (N'ᾰ',N'ă'),
(N'Ᾱ',N'Ā'), (N'ᾱ',N'a'),
(N'ᾲ',N'a'),
 (N'ᾳ',N'ą'),
  (N'ᾴ',N'a'),
   (N'ᾶ',N'ã'),
   (N'ᾷ',N'a'),
(N'Ὰ',N'A'),
 (N'ᾼ',N'Ą'),
(N'ῂ',N'i'),
(N'ῃ',N'i'),
(N'ῄ',N'i'),
(N'ῆ',N'i'),
(N'Ὲ',N'E'),
(N'Ὴ',N'I'),
(N'ῌ',N'Į'),
(N'Ῐ',N'Ĭ'), (N'ῐ',N'ĭ'),
(N'Ῑ',N'Ī'), (N'ῑ',N'ī'),
(N'ῒ',N'i'),
(N'ῖ',N'ĩ'),
(N'ῗ',N'i'),
(N'Ὶ',N'i'),
(N'Ῠ',N'Y'), (N'ῠ',N'y'),
(N'Ῡ',N'Y'), (N'ῡ',N'y'),
(N'ῢ',N'y'),
(N'ῤ',N'r'),
(N'ῥ',N'r'),
(N'ῦ',N'y'),
(N'Ῥ',N'R'),
(N'ῲ',N'Ó'),
(N'ῼ',N'Ó'), (N'ῳ',N'ó'),
(N'ῴ',N'ó'),
(N'ῶ',N'ó'),
(N'ῷ',N'ó'),
(N'Ὸ',N'O'),
(N'Ὼ',N'Ó');	
GO
--Romanize function
PRINT 'CREATE FUNCTION [dbo].[Romanize]';
GO
--- <summary>Converts given Unicode string to Unicode string containing romanized characters</summary>
--- <param name="Str">String to be romenized</param>
--- <returns><paramref name="Str"> with characters for which romanization is know romanized</returns>
--- <remarks>Romanization is based on romanization rules tored in <see cref="dbo.Romanization"/> table</remarks>
CREATE FUNCTION [dbo].[Romanize]
(@Str NVARCHAR(MAX))
RETURNS NVARCHAR(MAX)
AS
BEGIN
    IF @Str IS NULL RETURN NULL;
    IF @Str = N'' RETURN N'';
	DECLARE @String NVARCHAR(MAX) = N'';
    DECLARE @i INT = 0;
    WHILE @i < LEN(@Str) BEGIN
        DECLARE @Char NCHAR(1) = SUBSTRING(@Str,@i+1,1);
        DECLARE @Romanization NVARCHAR(10);
        SET @Romanization = NULL;
        SET @Romanization = (SELECT r.Romanization FROM dbo.Romanization r WHERE r.[Code] = UNICODE(@Char));
        IF @Romanization IS NULL
			SET @String = @String + @Char;
		ELSE
			SET @String = @String + @Romanization;
        SET @i = @i + 1;
    END;
    RETURN @String;
END;
GO
--================================================================================================================================
--VarCharTable UDT (requires re-creation of GetSimilarCaps)
PRINT 'DROP PROCEDURE dbo.GetSimilarCaps';
GO
DROP PROCEDURE dbo.GetSimilarCaps;
GO
PRINT 'DROP TYPE [dbo].[VarCharTable]';
GO
DROP TYPE [dbo].[VarCharTable];
GO
PRINT 'CREATE TYPE [dbo].[VarCharTable]';
GO
CREATE TYPE [dbo].[VarCharTable] AS TABLE(
	[Value] nvarchar(50) NOT NULL,
	PRIMARY KEY CLUSTERED 
(
	[Value] ASC
)WITH (IGNORE_DUP_KEY = OFF)
);
GO


--Re-cerate GetSimilarCaps
PRINT 'CREATE PROCEDURE [dbo].[GetSimilarCaps]';
GO
CREATE PROCEDURE [dbo].[GetSimilarCaps] 
	-- Add the parameters for the stored procedure here
	@CapTypeID int = null, 
	@MainTypeID int = null,
	@ShapeID int = null,
	@CapName varchar(255) = null,
	@MainText varchar(255) = null,
	@SubTitle varchar(255) = null,
	@BackColor1 int = null,
	@BackColor2 int = null,
	@ForeColor int = null,
	@MainPicture varchar(255) = null,
	@TopText varchar(max)=null,
	@SideText varchar(max)=null,
	@BottomText varchar(max)=null,
	@MaterialID int =null,
	@Surface char(1) =null,
	@Size int = null,
	@Size2 int = null,
	@Height int = null,
	@Is3D bit = null,
	@Year int = null,
	@CountryCode char(2)= null,
	@Note varchar(max)= null,
	@CompanyID int= null,
	@ProductID int = null,
	@ProductTypeID int = null,
	@StorageID int = null,
	@ForeColor2 int = null,
	@PictureType char(1) = null,
	@HasBottom bit =  null,
	@HasSide bit = null,
	@AnotherPictures varchar(max)= null,
	@CategoryIDs dbo.IntTable readonly,
	@Keywords dbo.VarCharTable readonly,
	@CountryOfOrigin char(3) = null,
	@IsDrink bit = null,
	@State smallint = null,
	@TargetID int = null,
	@IsAlcoholic bit = null,
	@CapSignIDs dbo.IntTable readonly
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT top 100 c.*,
			case when c.CapName = @CapName then 200 else 0 end
            +
            case when c.MainText = @MainText then 200 else 0 end 
            +
            case when c.SubTitle = @SubTitle then 200 else 0 end
            +
            case when c.CapTypeID = @CapTypeID then 50 else 0 end
            +
            case when c.MainTypeID = @MainTypeID then 20 else 0 end
            +
            case when c.ShapeID = @ShapeID then 20 else 0 end
            +
            case when c.BackColor1 = @BackColor1 then 40 else 0 end
            +
            case when c.BackColor2 = @BackColor2  then 50 else 0 end 
            +
            case when c.ForeColor = @ForeColor    then 50 else 0 end
            +
            case when c.ForeColor2 = @ForeColor   then 50 else 0 end
            +
            case when c.MainPicture = @MainPicture then 50 else 0 end
            +
            case when c.TopText = @TopText then 200 else 0 end
            +
            case when c.SideText = @SideText then 300 else 0 end
            +
            case when c.BottomText = @BottomText then 300 else 0 end
            +
            case when c.MaterialID = @MaterialID then 20 else 0 end
            +
            case when c.Surface = @Surface then 10 else 0 end
            +
            case when c.ShapeID = @ShapeID and abs(c.Size-@Size)<3 then 10 - abs(c.Size-@Size) / 0.4  else 0 end
            +
            case when c.ShapeID = @ShapeID and abs(c.Size2-@Size2)<3 then 10 - abs(c.Size2-@Size2) / 0.4 else 0 end
            +
            case when abs(c.Height - @Height) < 3 then 10 - abs(c.Height - @Height) / 0.4  else 0 end
            +
            case when c.Is3D = @Is3D and c.Is3D = 1 then 200 when c.Is3D=@Is3D then 10 else 0 end
            +
            case when c.Year = @Year then 10 else 0 end
            +
            case when c.CountryCode = @CountryCode then 10 else 0 end
            +
            case when c.Note = @Note then 20 else 0 end
            +
            case when c.CompanyID = @CompanyID then 50 else 0 end
            +
            case when c.ProductID = @ProductID then 50 else 0   end
            +
            case when c.ProductTypeID=@ProductTypeID then 20 else 0 end
            +
            case when c.StorageID=@StorageID then 10 else 0 end
            +
            case when c.PictureType=@PictureType then 10 else 0 end
            +
            case when c.HasBottom=@HasBottom and c.HasBottom = 1 then 50 when c.HasBottom=@HasBottom then 10 else 0 end
            +
            case when c.HasSide=@HasSide and c.HasSide = 1 then 40 when c.HasSide=@HasSide then 10 else 0 end
            +
            case when c.AnotherPictures = @AnotherPictures then 50 else 0 end
            +
            (select count(*) from dbo.cap_category_int cci where cci.capid=c.capid and cci.categoryid in(select value from @categoryids))*30
            +
            (select count(*) from dbo.cap_keyword_int cki inner join dbo.keyword k on(cki.keywordid=k.keywordid) where cki.CapID=c.CapID and  k.keyword in(select value from @keywords))*40
            +
            case when c.CountryOfOrigin = @CountryOfOrigin then 20 else 0 end
            +
            case when c.IsDrink = @IsDrink and @IsDrink=0 then 30 when c.IsDrink = @IsDrink then 10 else 0 end
            +
            case when c.State = @State then 10 else 0 end
            +
            case when c.TargetID = @TargetID then 20 else 0 end
            +
            case when c.IsDrink = 1 and @IsDrink = 1 and @IsAlcoholic = c.IsAlcoholic and @IsAlcoholic = 1 then 30
                 when c.IsDrink = 1 and @IsDrink = 1 and @IsAlcoholic = c.IsAlcoholic then 10 else 0 end
            +
            (select COUNT(*) from dbo.Cap_CapSign_Int csi where csi.CapID=csi.CapID and csi.CapSignID in(select value from @CapSignIDs))*20
            as Score
		from dbo.Cap c
        order by
            Score desc
            ;
END;
GO

--================================================================================================================================
-------------------------------- Translation-related procedures -----------------------------------------------------------------
GO
--TranslateSimplaObject
PRINT 'CREATE PROCEDURE [dbo].[TranslateSimpleObject]';
GO					   
--- <summary>Returns full translation of object that supports simple translation</summary>
--- <param name="ObjectType">String type of object to get simple translation of. This must be name of one of table table <see cref="dbo.SimpleTranslation"/> has relation to.</param>
--- <param name="ObjectID">ID of object to get translation of</param>
--- <param name="CultureNames">Contains names of cultures to get translation for. Cultures are evaluated in order they are passed to this parameter and 1st culture that object has translation for is used. If object does not have translation for any of given cultures invariant data are read from object itself.</param>
--- <returns>One row with following columns
--- <list type="table"><listheader><term>Column name</term><description>Description</description></listheader>
--- <item><term>Name</term><description>Localized name of the object</description></item>
--- <item><term>NameCulture</term><description>Name of culture Name is returned in. Null if Name was read from object itself.</description></item>
--- <item><term>Description</term><description>Localized description of the object</description></item>
--- <item><term>DescriptionCulture</term><description>Name of culture Description is returned in. Null if Description was read from object itself.</description></item>
--- </list></returns>
CREATE PROCEDURE [dbo].[TranslateSimpleObject]
	(
	@ObjectType varchar(15),
	@ObjectID int,
	@CultureNames dbo.VarCharTable READONLY
	)
AS
BEGIN

	DECLARE @Results TABLE(
		ID int IDENTITY(1,1) PRIMARY KEY,
		[SimpleTranslationID] [int] NULL,
		[CategoryID] [int] NULL,
		[KeywordID] [int] NULL,
		[ProductID] [int] NULL,
		[CompanyID] [int] NULL,
		[ProductTypeID] [int] NULL,
		[TargetID] [int] NULL,
		[MaterialID] [int] NULL,
		[CapTypeID] [int] NULL,
		[MainTypeID] [int] NULL,
		[CapSignID] [int] NULL,
		[StorageID] [int] NULL,
		[StorageTypeID] [int] NULL,
		[CapInstanceID] [int] NULL,
		[Culture] [varchar](15) NULL,
		[Name] [nvarchar](50) NULL,
		[Description] [nvarchar](max) NULL
	);
	DECLARE @CulturesOrdered TABLE(ID int IDENTITY(1,1) PRIMARY KEY, Culture varchar(15) UNIQUE);
    INSERT INTO @CulturesOrdered(Culture) SELECT cn.Value FROM @CultureNames cn;
   
	INSERT INTO @Results ([SimpleTranslationID],[CategoryID],[KeywordID],[ProductID],[CompanyID],[ProductTypeID],[TargetID],[MaterialID],[CapTypeID],[MainTypeID],[CapSignID],[StorageID],[StorageTypeID],[CapInstanceID],[Culture],[Name],[Description])
		SELECT st.* FROM dbo.SimpleTranslation st  INNER JOIN @CulturesOrdered co ON(st.Culture = co.Culture)
		WHERE ((@ObjectType = 'Category' AND st.CategoryID = @ObjectID) OR
			(@ObjectType = 'Keyword' AND st.KeywordID = @ObjectID) OR
			(@ObjectType = 'Product' AND st.ProductID = @ObjectID) OR
			(@ObjectType = 'Company' AND st.CompanyID = @ObjectID) OR
			(@ObjectType = 'ProductType' AND st.ProductTypeID = @ObjectID) OR
			(@ObjectType = 'Target' AND st.TargetID = @ObjectID) OR
			(@ObjectType = 'Material' AND st.MaterialID = @ObjectID) OR
			(@ObjectType = 'CapType' AND st.CapTypeID = @ObjectID) OR
			(@ObjectType = 'MainType' AND st.MainTypeID = @ObjectID) OR
			(@ObjectType = 'CapSign' AND st.CapSignID = @ObjectID) OR
			(@ObjectType = 'Storage' AND st.StorageID = @ObjectID) OR
			(@ObjectType = 'StorageType' AND st.StorageTypeID = @ObjectID) OR
			(@ObjectType = 'CapInstance' AND st.CapInstanceID = @ObjectID));
     
	IF @ObjectType = 'Category' 
		INSERT INTO @Results (CategoryID, Name, [Description]) SELECT t.CategoryID, t.CategoryName, t.[Description] FROM Category t WHERE t.CategoryID = @ObjectID;
	ELSE IF @ObjectType = 'Keyword' 
		INSERT INTO @Results (KeywordID, Name, [Description]) SELECT t.KeywordID, t.Keyword, NULL FROM Keyword t WHERE t.KeywordID = @ObjectID;
	ELSE IF @ObjectType = 'Product' 
		INSERT INTO @Results (ProductID, Name, [Description]) SELECT t.ProductID, t.ProductName, t.[Description] FROM Product t WHERE t.ProductID = @ObjectID;
	ELSE IF @ObjectType = 'Company' 
		INSERT INTO @Results (CompanyID, Name, [Description]) SELECT t.CompanyID, t.CompanyName, t.[Description] FROM Company t WHERE t.CompanyID = @ObjectID;
	ELSE IF @ObjectType = 'ProductType' 
		INSERT INTO @Results (ProductTypeID, Name, [Description]) SELECT t.ProductTypeID, t.ProductTypeName, t.[Description] FROM ProductType t WHERE t.ProductTypeID = @ObjectID;
	ELSE IF @ObjectType = 'Target' 
		INSERT INTO @Results (TargetID, Name, [Description]) SELECT t.TargetID, t.Name, t.[Description] FROM Target t WHERE t.TargetID = @ObjectID;
	ELSE IF @ObjectType = 'Material' 
		INSERT INTO @Results (MaterialID, Name, [Description]) SELECT t.MaterialID, t.Name, t.[Description] FROM Material t WHERE t.MaterialID = @ObjectID;
	ELSE IF @ObjectType = 'CapType' 
		INSERT INTO @Results (CapTypeID, Name, [Description]) SELECT t.CapTypeID, t.TypeName, t.[Description] FROM CapType t WHERE t.CapTypeID = @ObjectID;
	ELSE IF @ObjectType = 'MainType' 
		INSERT INTO @Results (MainTypeID, Name, [Description]) SELECT t.MainTypeID, t.TypeName, t.[Description] FROM MainType t WHERE t.MainTypeID = @ObjectID;
	ELSE IF @ObjectType = 'CapSign' 
		INSERT INTO @Results (CapSignID, Name, [Description]) SELECT t.CapSignID, t.Name, t.[Description] FROM CapSign t WHERE t.CapSignID = @ObjectID;
	ELSE IF @ObjectType = 'Storage' 
		INSERT INTO @Results (StorageID, Name, [Description]) SELECT t.StorageID, t.StorageNumber, t.[Description] FROM Storage t WHERE t.StorageID = @ObjectID;
	ELSE IF @ObjectType = 'StorageType' 
		INSERT INTO @Results (StorageTypeID, Name, [Description]) SELECT t.StorageTypeID, t.Name, t.[Description] FROM StorageType t WHERE t.StorageTypeID = @ObjectID;
	ELSE IF @ObjectType = 'CapInstance' 
		INSERT INTO @Results (CapInstanceID, Name, [Description]) SELECT t.CapInstanceID, NULL, t.Note FROM CapInstance t WHERE t.CapInstanceID = @ObjectID;

	SELECT * FROM
		(SELECT TOP 1 Name, Culture AS NameCulture FROM @Results WHERE Name IS NOT NULL ORDER BY ID) tName	
		CROSS JOIN
		(SELECT TOP 1 [Description], Culture AS DescriptionCulture FROM @Results WHERE [Description] IS NOT NULL ORDER BY ID) tCulture;
END;
GO
--TranslateCap
PRINT 'CREATE PROCEDURE [dbo].[TranslateCap]';
GO
--- <summary>Returns full translation of <see cref="dbo.Cap"/> object</summary>
--- <param name="CapID">ID of <see cref="dbo.Cap"/> to get translation of</param>
--- <param name="CultureNames">Contains names of cultures to get translation for. Cultures are evaluated in order they are passed to this parameter and 1st culture that object has translation for is used. If object does not have translation for any of given cultures invariant data are read from object itself.</param>
--- <returns>One row with following columns
--- <list type="table"><listheader><term>Column name</term><description>Description</description></listheader>
--- <item><term>CapID</term><description><paramref name="CapID"/></description></item>
--- <item><term>CapName</term><description>Localized name of the cap</description></item>
--- <item><term>CapNameCulture</term><description>Name of culture CapName is returned in. Null if Name was read diersctly from <see cref="dbo.Cap"/>.</description></item>
--- <item><term>MainPicture</term><description>Localized description of cap main picture</description></item>
--- <item><term>MainPictureCulture</term><description>Name of culture MainPicture is returned in. Null if Name was read diersctly from <see cref="dbo.Cap"/>.</description></item>
--- <item><term>Note</term><description>Localized note on cap</description></item>
--- <item><term>NoteCulture</term><description>Name of culture Note is returned in. Null if Name was read diersctly from <see cref="dbo.Cap"/>.</description></item>
--- <item><term>AnotherPictures</term><description>Localized description of other pictures on shape</description></item>
--- <item><term>AnotherPicturesCulture</term><description>Name of culture AnotherPictures is returned in. Null if Description was read diersctly from <see cref="dbo.Cap"/>.</description></item>
--- </list></returns>
CREATE PROCEDURE [dbo].[TranslateCap](
	@CapID int,
	@CultureNames dbo.VarCharTable READONLY
) AS BEGIN

	DECLARE @Results TABLE(
		ID int IDENTITY(1,1) PRIMARY KEY,
		[CapTranslationID] [int] NULL,
		[CapID] [int] NULL,
		[Culture] [varchar](15) NULL,
		[CapName] [nvarchar](255) NULL,
		MainPicture nvarchar(255) NULL,
		Note nvarchar(MAX) NULL,
		AnotherPictures [nvarchar](max) NULL
	);
	DECLARE @CulturesOrdered TABLE(ID int IDENTITY(1,1) PRIMARY KEY, Culture varchar(15) UNIQUE);
    INSERT INTO @CulturesOrdered(Culture) SELECT cn.Value FROM @CultureNames cn;
   
	INSERT INTO @Results (CapTranslationID,CapID,Culture,CapName,MainPicture,Note,AnotherPictures)
		SELECT ct.* FROM  dbo.CapTranslation ct INNER JOIN @CulturesOrdered co ON(ct.Culture = co.Culture)
		WHERE ct.CapID = @CapID
		ORDER BY co.ID;
	INSERT INTO @Results (CapID,CapName,MainPicture,Note,AnotherPictures)
		SELECT c.CapID,c.CapName,c.MainPicture,c.Note,c.AnotherPictures FROM Cap c WHERE c.CapID = @CapID   ;


	SELECT @CapID AS CapID, tName.*, tMainPicture.*, tNote.*, tAnotherPictures.* FROM
		(SELECT TOP 1 CapName, Culture AS CapNameCulture FROM @Results WHERE CapName IS NOT NULL ORDER BY ID) tName	
		CROSS JOIN
		(SELECT TOP 1 MainPicture, Culture AS MainPictureCulture FROM @Results WHERE MainPicture IS NOT NULL ORDER BY ID) tMainPicture	
		CROSS JOIN
		(SELECT TOP 1 Note, Culture AS NoteCulture FROM @Results WHERE Note IS NOT NULL ORDER BY ID) tNote
		CROSS JOIN
		(SELECT TOP 1 AnotherPictures, Culture AS AnotherPicturesCulture FROM @Results WHERE AnotherPictures IS NOT NULL ORDER BY ID) tAnotherPictures
		;
END;
GO
--TranslateShape
PRINT 'CREATE PROCEDURE [dbo].[TranslateShape]';
GO	 
--- <summary>Returns full translation of <see cref="dbo.Shape"/> object</summary>
--- <param name="ShapeID">ID of <see cref="dbo.Shape"/> to get translation of</param>
--- <param name="CultureNames">Contains names of cultures to get translation for. Cultures are evaluated in order they are passed to this parameter and 1st culture that object has translation for is used. If object does not have translation for any of given cultures invariant data are read from object itself.</param>
--- <returns>One row with following columns
--- <list type="table"><listheader><term>Column name</term><description>Description</description></listheader>
--- <item><term>ShapeID</term><description><paramref name="ShapeID"/></description></item>
--- <item><term>Name</term><description>Localized name of the shape</description></item>
--- <item><term>NameCulture</term><description>Name of culture Name is returned in. Null if Name was read diersctly from <see cref="dbo.Shape"/>.</description></item>
--- <item><term>Size1Name</term><description>Localized name of primary size of the shape</description></item>
--- <item><term>Size1NameCulture</term><description>Name of culture Size1Name is returned in. Null if Name was read diersctly from <see cref="dbo.Shape"/>.</description></item>
--- <item><term>Size2Name</term><description>Localized name of secondary size of the shape</description></item>
--- <item><term>Size2NameCulture</term><description>Name of culture Size2Name is returned in. Null if Name was read diersctly from <see cref="dbo.Shape"/>.</description></item>
--- <item><term>Description</term><description>Localized description of the shape</description></item>
--- <item><term>DescriptionCulture</term><description>Name of culture Description is returned in. Null if Description was read diersctly from <see cref="dbo.Shape"/>.</description></item>
--- </list></returns>
CREATE PROCEDURE [dbo].[TranslateShape](
	@ShapeID int,
	@CultureNames dbo.VarCharTable READONLY
) AS BEGIN

	DECLARE @Results TABLE(
		ID int IDENTITY(1,1) PRIMARY KEY,
		[ShapeTranslationID] [int] NULL,
		[ShapeID] [int] NULL,
		[Culture] [varchar](15) NULL,
		[Name] [nvarchar](50) NULL,
		Size1Name nvarchar(50) NULL,
		Size2Name nvarchar(50) NULL,
		[Description] [nvarchar](max) NULL
	);
	DECLARE @CulturesOrdered TABLE(ID int IDENTITY(1,1) PRIMARY KEY, Culture varchar(15) UNIQUE);
    INSERT INTO @CulturesOrdered(Culture) SELECT cn.Value FROM @CultureNames cn;
   
	INSERT INTO @Results (ShapeTranslationID,ShapeID,Culture,Name,Size1Name,Size2Name,[Description])
		SELECT st.* FROM  dbo.ShapeTranslation st INNER JOIN @CulturesOrdered co ON(st.Culture = co.Culture)
		WHERE st.ShapeID = @ShapeID
		ORDER BY co.ID;
	INSERT INTO @Results (ShapeID,Name,Size1Name,Size2Name,[Description])
		SELECT s.ShapeID,s.Name,s.Size1Name,s.Size2Name,s.[Description] FROM Shape s WHERE s.ShapeID = @ShapeID


	SELECT @ShapeID AS ShapeID, tName.*, tSize1Name.*, tSize2Name.*, tDescription.* FROM
		(SELECT TOP 1 Name, Culture AS NameCulture FROM @Results WHERE Name IS NOT NULL ORDER BY ID) tName	
		CROSS JOIN
		(SELECT TOP 1 Size1Name, Culture AS Size1NameCulture FROM @Results WHERE Size1Name IS NOT NULL ORDER BY ID) tSize1Name	
		CROSS JOIN
		(SELECT TOP 1 Size2Name, Culture AS Size2NameCulture FROM @Results WHERE Size2Name IS NOT NULL ORDER BY ID) tSize2Name	
		CROSS JOIN
		(SELECT TOP 1 [Description], Culture AS DescriptionCulture FROM @Results WHERE [Description] IS NOT NULL ORDER BY ID) tDescription
		;
END;
GO
-------------------------------------------------------------------------------------------------------------------------------
--Cleanup
PRINT 'Cleanup';
EXEC sp_rename N'category_Instead_Upd',N'Category_Instead_Upd';
EXEC sp_rename N'company_Instead_Upd',N'Company_Instead_Upd';
EXEC sp_rename N'image_Instead_Upd',N'Image_Instead_Upd';
EXEC sp_rename N'material_Instead_Upd',N'Material_Instead_Upd';
EXEC sp_rename N'product_Instead_Upd',N'Product_Instead_Upd';
EXEC sp_rename N'productype_Instead_Upd',N'ProducType_Instead_Upd';
EXEC sp_rename N'shape_Instead_Upd',N'Shape_Instead_Upd';
EXEC sp_rename N'storagetype_Instead_Upd',N'StorageType_Instead_Upd';
EXEC sp_rename N'target_Instead_Upd',N'Target_Instead_Upd';
EXEC sp_rename N'storage_Instead_Upd',N'Storage_Instead_Upd';
EXEC sp_rename N'CK_Cap_CountryOfOrigin',N'CHK_Cap_CountryOfOrigin';
GO
--------------------------------------------------------------------------------------------------------------------------------
--Triggers
GO
ALTER TRIGGER [dbo].[Cap_Instead_Upd] ON  dbo.Cap INSTEAD OF UPDATE AS 
BEGIN
	SET NOCOUNT ON;
	  UPDATE [dbo].[Cap]
   SET [CapTypeID] = i.CapTypeID
      ,[MainTypeID] = i.MainTypeID
      ,[ShapeID] = i.ShapeID
      ,[CapName] = dbo.EmptyStrToNull(i.CapName)
      ,[MainText] = dbo.EmptyStrToNull(i.MainText)
      ,[SubTitle] = dbo.EmptyStrToNull(i.SubTitle)
      ,[BackColor1] = i.BackColor1
      ,[BackColor2] = i.BackColor2
      ,[ForeColor] = i.ForeColor
      ,[MainPicture] = dbo.EmptyStrToNull(i.MainPicture)
      ,[TopText] = dbo.EmptyStrToNull(i.TopText)
      ,[SideText] = dbo.EmptyStrToNull(i.SideText)
      ,[BottomText] = dbo.EmptyStrToNull(i.BottomText)
      ,[MaterialID] = i.MaterialID
      ,[Surface] = i.Surface
      ,[Size] = i.Size
      ,[Size2] = i.Size2
      ,[Height] = i.Height
      ,[Is3D] = i.Is3D
      ,[Year] = i.Year
      ,[CountryCode] = dbo.EmptyStrToNull(i.CountryCode)
      ,[Note] = dbo.EmptyStrToNull(i.Note)
      ,[CompanyID] = i.CompanyID
      ,[ProductID] = i.ProductID
      ,[ProductTypeID] = i.ProductTypeID
      ,[StorageID] = i.StorageID
      ,[ForeColor2] = i.ForeColor2
      ,[PictureType] = i.PictureType
      ,[HasBottom] = i.HasBottom
      ,[HasSide] = i.HasSide
      ,[AnotherPictures] = dbo.EmptyStrToNull(i.AnotherPictures),
      countryoforigin=i.countryoforigin,
      isdrink=i.isdrink,
      [state]=i.[state],
      targetid=i.targetid,
      isalcoholic=i.isalcoholic
 FROM inserted AS i
 WHERE cap.capid=i.capid;
END
GO
ALTER TRIGGER [dbo].[Cap_Instead_Ins] ON  dbo.Cap INSTEAD OF INSERT AS 
BEGIN
	SET NOCOUNT ON;
INSERT INTO dbo.cap 
					 (	[CapTypeID]
      ,[MainTypeID]
      ,[ShapeID]
      ,[CapName]
      ,[MainText]
      ,[SubTitle]
      ,[BackColor1]
      ,[BackColor2]
      ,[ForeColor]
      ,[MainPicture]
      ,[TopText]
      ,[SideText]
      ,[BottomText]
      ,[MaterialID]
      ,[Surface]
      ,[Size]
      ,[Size2]
      ,[Height]
      ,[Is3D]
      ,[Year]
      ,[CountryCode]
      ,[DateCreated]
      ,[Note]
      ,[CompanyID]
      ,[ProductID]
      ,[ProductTypeID]
      ,[StorageID]
      ,[ForeColor2]
      ,[PictureType]
      ,[HasBottom]
      ,[HasSide]
      ,[AnotherPictures],Countryoforigin,isdrink,[state],targetid,isalcoholic)
                   output INSERTED.*
			 SELECT [CapTypeID]
      ,[MainTypeID]
      ,[ShapeID]
      ,dbo.EmptyStrToNull([CapName])
      ,dbo.EmptyStrToNull([MainText])
      ,dbo.EmptyStrToNull([SubTitle])
      ,[BackColor1]
      ,[BackColor2]
      ,[ForeColor]
      ,dbo.EmptyStrToNull([MainPicture])
      ,dbo.EmptyStrToNull([TopText])
      ,dbo.EmptyStrToNull([SideText])
      ,dbo.EmptyStrToNull([BottomText])
      ,[MaterialID]
      ,[Surface]
      ,[Size]
      ,[Size2]
      ,[Height]
      ,[Is3D]
      ,[Year]
      ,dbo.EmptyStrToNull([CountryCode])
      ,isnull([DateCreated],getdate())
      ,dbo.EmptyStrToNull([Note])
      ,[CompanyID]
      ,[ProductID]
      ,[ProductTypeID]
      ,[StorageID]
      ,[ForeColor2]
      ,dbo.EmptyStrToNull([PictureType])
      ,[HasBottom]
      ,[HasSide]
      ,dbo.EmptyStrToNull([AnotherPictures]),
      dbo.EmptyStrToNull(Countryoforigin),isdrink,[state],targetid, isalcoholic
  FROM inserted;
END
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
	DECLARE @dbVersion NVARCHAR(11) = '0.1.4.0';
	RETURN @dbGuid + @dbversion;
END;   
GO
PRINT 'Done!';
GO
COMMIT;