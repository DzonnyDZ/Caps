﻿-- New table CapSign
CREATE TABLE  [dbo].[CapSign](
	[CapSignID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_CapSign] PRIMARY KEY CLUSTERED 
(
	[CapSignID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[CapSign]  WITH CHECK ADD  CONSTRAINT [CHK_CapSign_NoEmptyStrings] CHECK  (([Name]<>'' AND [Description]<>''))
GO

ALTER TABLE [dbo].[CapSign] CHECK CONSTRAINT [CHK_CapSign_NoEmptyStrings]
GO

create TRIGGER [dbo].[CapSign_Instead_Ins] 
   ON  [dbo].[CapSign]
   instead of INSERT
AS 
BEGIN
	SET NOCOUNT ON;
			 insert into dbo.CapSign
					 (	 Name,[Description])
          output inserted.*
			 SELECT Name,dbo.EmptyStrToNull([Description])
  FROM inserted	 ;
END
GO
create TRIGGER [dbo].[Capsign_Instead_Upd]
   ON  [dbo].[CapSign] 
   instead of update
AS 
BEGIN
	SET NOCOUNT ON;
	  UPDATE [dbo].CapSign
   SET	
			name=i.name,
      Description=dbo.EmptyStrToNull(i.description)
 from inserted	as i
 WHERE Capsign.capsignid=i.capsignid;
END
GO

--Add missing foreign keys
--it somehow happened that previous version was missing these relations
ALTER TABLE [dbo].[Cap]  WITH CHECK ADD  CONSTRAINT [FK_Cap_CapType] FOREIGN KEY([CapTypeID])
REFERENCES [dbo].[CapType] ([CapTypeID])
GO
ALTER TABLE [dbo].[Cap] CHECK CONSTRAINT [FK_Cap_CapType]
GO

ALTER TABLE [dbo].[Cap]  WITH CHECK ADD  CONSTRAINT [FK_Cap_MainType] FOREIGN KEY([MainTypeID])
REFERENCES [dbo].[MainType] ([MainTypeID])
GO
ALTER TABLE [dbo].[Cap] CHECK CONSTRAINT [FK_Cap_MainType]
GO

ALTER TABLE dbo.Cap WITH CHECK ADD CONSTRAINT FK_Cap_Shape FOREIGN KEY ( ShapeID )
REFERENCES dbo.Shape ( ShapeID )
GO
ALTER TABLE [dbo].[Cap] CHECK CONSTRAINT FK_Cap_Shape
GO

ALTER TABLE [dbo].[Cap]  WITH CHECK ADD  CONSTRAINT [FK_Cap_Material] FOREIGN KEY([MaterialID])
REFERENCES [dbo].[Material] ([MaterialID])
GO
ALTER TABLE [dbo].[Cap] CHECK CONSTRAINT [FK_Cap_Material]
GO

ALTER TABLE dbo.Cap WITH CHECK ADD CONSTRAINT FK_Cap_Product FOREIGN KEY ( ProductID )
REFERENCES dbo.Product ( ProductID ) 
GO
ALTER TABLE [dbo].[Cap] CHECK CONSTRAINT [FK_Cap_Product]
GO

ALTER TABLE dbo.Cap WITH CHECK ADD CONSTRAINT FK_Cap_ProductType FOREIGN KEY ( ProductTypeID )
REFERENCES dbo.ProductType ( ProductTypeID ) 
GO
ALTER TABLE [dbo].[Cap] CHECK CONSTRAINT [FK_Cap_ProductType]
GO

ALTER TABLE dbo.Cap WITH CHECK ADD CONSTRAINT FK_Cap_Company FOREIGN KEY ( CompanyID )
REFERENCES dbo.Company ( CompanyID ) 
GO
ALTER TABLE [dbo].[Cap] CHECK CONSTRAINT [FK_Cap_Company]
GO

ALTER TABLE dbo.Cap WITH CHECK ADD CONSTRAINT FK_Cap_Storage FOREIGN KEY ( StorageID )
REFERENCES dbo.Storage ( StorageID )
GO
ALTER TABLE [dbo].[Cap] CHECK CONSTRAINT [FK_Cap_Storage]
GO

ALTER TABLE dbo.Cap WITH CHECK ADD CONSTRAINT FK_Cap_Target FOREIGN KEY ( TargetID )
REFERENCES dbo.Target( TargetID ) 	
GO
ALTER TABLE [dbo].[Cap] CHECK CONSTRAINT [FK_Cap_Target]
GO

-- New relation Cap-CapSign
ALTER TABLE dbo.Cap ADD CapSignID INT;
GO
ALTER TABLE [dbo].[Cap]  WITH CHECK ADD  CONSTRAINT [FK_Cap_CapSign] FOREIGN KEY([CapSignID])
REFERENCES [dbo].[CapSign] ([CapSignID])
GO
ALTER TABLE [dbo].[Cap] CHECK CONSTRAINT [FK_Cap_CapSign]
GO

-- Alter procedure GetSimilarCaps
ALTER PROCEDURE [dbo].[GetSimilarCaps] 
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
	@CapSignId int = null
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
            case when c.CapSignID = @CapSignId then 20 else 0 end
            as Score
		from dbo.Cap c
        order by
            Score desc
            ;
END
GO

--Alter triggers
ALTER TRIGGER [dbo].[Cap_Instead_Ins] 
   ON  dbo.Cap 
   instead of INSERT
AS 
BEGIN
	SET NOCOUNT ON;
insert into dbo.cap 
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
      ,[AnotherPictures],Countryoforigin,isdrink,[state],targetid,isalcoholic,capsignid)
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
      dbo.EmptyStrToNull(Countryoforigin),isdrink,[state],targetid, isalcoholic,capsignid
  FROM inserted	 ;
  
END
GO
ALTER TRIGGER [dbo].[Cap_Instead_Upd] 
   ON  dbo.Cap 
   instead of update
AS 
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
     -- ,[DateCreated] = i.DateCreated
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
      isalcoholic=i.isalcoholic,
      capsignid=i.capsignid
 from inserted	as i
 WHERE cap.capid=i.capid;
	 
END
GO


--Enforce sign name uniqueness
BEGIN --This is only for development database (version w/o UNQ have not been published)
    BEGIN TRANSACTION;
    UPDATE dbo.CapSign SET [Name] = [Name] + ' (' + CAST(CapSignID AS VARCHAR) + ')'
    WHERE EXISTS (SELECT * FROM dbo.CapSign t2 WHERE CapSign.Name = t2.Name AND CapSign.CapSignID <> t2.CapSignID);
    COMMIT;
END;  
GO

ALTER TABLE dbo.CapSign ADD CONSTRAINT
	UNQ_CapSign_Name UNIQUE NONCLUSTERED  ( Name )
	WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY];
GO

    --Increase version
    ALTER FUNCTION [dbo].[GetDatabaseVersion] ()
    RETURNS nvarchar(50)
    AS
    BEGIN
    declare @dbGuid nvarchar(38) = '{DAFDAE3F-2F0A-4359-81D6-50BA394D72D9}';
    declare @dbVersion nvarchar(11) = '0.1.3.0';
    return @dbGuid + @dbversion;

    END;   
    GO