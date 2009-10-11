-- New table CapSign
CREATE TABLE [dbo].[CapSign](
	[CapSignId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_CapSign] PRIMARY KEY CLUSTERED 
(
	[CapSignId] ASC
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
	  UPDATE [dbo].CapSing
   SET	
			name=i.name,
      Description=dbo.EmptyStrToNull(i.description)
 from inserted	as i
 WHERE Capsign.targetid=i.targetid;
END


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

ALTER TABLE dbo.Cap ADD CONSTRAINT FK_Cap_Shape FOREIGN KEY ( ShapeID )
REFERENCES dbo.Shape ( ShapeID )
GO
ALTER TABLE dbo.Cap SET (LOCK_ESCALATION = TABLE)
GO

ALTER TABLE [dbo].[Cap]  WITH CHECK ADD  CONSTRAINT [FK_Cap_Material] FOREIGN KEY([MaterialID])
REFERENCES [dbo].[Material] ([MaterialID])
GO
ALTER TABLE [dbo].[Cap] CHECK CONSTRAINT [FK_Cap_Material]
GO

ALTER TABLE dbo.Cap ADD CONSTRAINT FK_Cap_Product FOREIGN KEY ( ProductID )
REFERENCES dbo.Product ( ProductID ) 
ALTER TABLE [dbo].[Cap] CHECK CONSTRAINT [FK_Cap_Product]
GO

ALTER TABLE dbo.Cap ADD CONSTRAINT FK_Cap_ProductType FOREIGN KEY ( ProductTypeID )
REFERENCES dbo.ProductType ( ProductTypeID ) 
GO
ALTER TABLE [dbo].[Cap] CHECK CONSTRAINT [FK_Cap_ProductType]
GO

ALTER TABLE dbo.Cap ADD CONSTRAINT FK_Cap_Company FOREIGN KEY ( CompanyID )
REFERENCES dbo.Company ( CompanyID ) 
GO
ALTER TABLE [dbo].[Cap] CHECK CONSTRAINT [FK_Cap_Company]
GO

ALTER TABLE dbo.Cap ADD CONSTRAINT FK_Cap_Storage FOREIGN KEY ( StorageID )
REFERENCES dbo.Storage ( StorageID )
GO
ALTER TABLE [dbo].[Cap] CHECK CONSTRAINT [FK_Cap_Storage]

GO
ALTER TABLE dbo.Cap ADD CONSTRAINT FK_Cap_Target FOREIGN KEY ( TargetID )
REFERENCES dbo.Target( TargetID ) 	
GO
ALTER TABLE [dbo].[Cap] CHECK CONSTRAINT [FK_Cap_Target]
GO

-- New relation Cap-CapSign
ALTER TABLE [dbo].[Cap]  WITH CHECK ADD  CONSTRAINT [FK_Cap_CapSign] FOREIGN KEY([CapSignID])
REFERENCES [dbo].[CapSign] ([CapSignId])
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
			case when c.CapName = @CapName then 20 else 0 end
            +
            case when c.MainText = @MainText then 20 else 0 end 
            +
            case when c.SubTitle = @SubTitle then 20 else 0 end
            +
            case when c.CapTypeID = @CapTypeID then 5 else 0 end
            +
            case when c.MainTypeID = @MainTypeID then 2 else 0 end
            +
            case when c.ShapeID = @ShapeID then 2 else 0 end
            +
            case when c.BackColor1 = @BackColor1 then 4 else 0 end
            +
            case when c.BackColor2 = @BackColor2  then 5 else 0 end 
            +
            case when c.ForeColor = @ForeColor    then 5 else 0 end
            +
            case when c.ForeColor2 = @ForeColor   then 5 else 0 end
            +
            case when c.MainPicture = @MainPicture then 5 else 0 end
            +
            case when c.TopText = @TopText then 20 else 0 end
            +
            case when c.SideText = @SideText then 30 else 0 end
            +
            case when c.BottomText = @BottomText then 30 else 0 end
            +
            case when c.MaterialID = @MaterialID then 2 else 0 end
            +
            case when c.Surface = @Surface then 1 else 0 end
            +
            case when c.ShapeID = @ShapeID and c.Size=@Size then 1 else 0 end
            +
            case when c.ShapeID = @ShapeID and c.Size2=@Size2 then 1 else 0 end
            +
            case when c.Height = @Height then 1 else 0 end
            +
            case when c.Is3D = @Is3D and c.Is3D = 1 then 20 when c.Is3D=@Is3D then 1 else 0 end
            +
            case when c.Year = @Year then 1 else 0 end
            +
            case when c.CountryCode = @CountryCode then 1 else 0 end
            +
            case when c.Note = @Note then 2 else 0 end
            +
            case when c.CompanyID = @CompanyID then 5 else 0 end
            +
            case when c.ProductID = @ProductID then 5 else 0   end
            +
            case when c.ProductTypeID=@ProductTypeID then 2 else 0 end
            +
            case when c.StorageID=@StorageID then 1 else 0 end
            +
            case when c.PictureType=@PictureType then 1 else 0 end
            +
            case when c.HasBottom=@HasBottom and c.HasBottom = 1 then 5 when c.HasBottom=@HasBottom then 1 else 0 end
            +
            case when c.HasSide=@HasSide and c.HasSide = 1 then 4 when c.HasSide=@HasSide then 1 else 0 end
            +
            case when c.AnotherPictures = @AnotherPictures then 5 else 0 end
            +
            (select count(*) from dbo.cap_category_int cci where cci.capid=c.capid and cci.categoryid in(select value from @categoryids))*3
            +
            (select count(*) from dbo.cap_keyword_int cki inner join dbo.keyword k on(cki.keywordid=k.keywordid) where cki.CapID=c.CapID and  k.keyword in(select value from @keywords))*4
            +
            case when c.CountryOfOrigin = @CountryOfOrigin then 2 else 0 end
            +
            case when c.IsDrink = @IsDrink and @IsDrink=0 then 3 when c.IsDrink = @IsDrink then 1 else 0 end
            +
            case when c.State = @State then 1 else 0 end
            +
            case when c.TargetID = @TargetID then 2 else 0 end
            +
            case when c.IsDrink = 1 and @IsDrink = 1 and @IsAlcoholic = c.IsAlcoholic and @IsAlcoholic = 1 then 3 when c.IsDrink = 1 and @IsDrink = 1 and @IsAlcoholic = c.IsAlcoholic then 1 else 0 end
            +
            case when c.CapSignID = @CapSignId then 2 else 0 end
            as Score
		from dbo.Cap c
        order by
            Score desc
            ;
END
GO

--Increase version
ALTER FUNCTION [dbo].[GetDatabaseVersion] ()
RETURNS nvarchar(50)
AS
BEGIN
declare @dbGuid nvarchar(38) = '{DAFDAE3F-2F0A-4359-81D6-50BA394D72D9}';
declare @dbVersion nvarchar(11) = '0.0.3.0';
return @dbGuid + @dbversion;

END;   
GO