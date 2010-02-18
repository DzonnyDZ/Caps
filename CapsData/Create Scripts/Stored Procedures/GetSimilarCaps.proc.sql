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