--- <summary>Searches for firts 100 caps similar to cap with given characteristics</summary>
--- <param name="CapTypeID">ID of cap type (<see cref="Cap.CapTypeID"/>)</param>
--- <param name="CapTypeID">ID of main type (<see cref="Cap.MainTypeID"/>)</param>
--- <param name="CapTypeID">ID of shape (<see cref="Cap.ShapeID"/>)</param>
--- <param name="CapName">Name of cap (<see cref="Cap.CapName"/>)</param>
--- <param name="MainText">Main text of cap (<see cref="Cap.MainText"/>)</param>
--- <param name="SubTitle">Secondary text of cap (<see cref="Cap.SubTitle"/>)</param>
--- <param name="BackColor1">Primary background color (RGB value, <see cref="Cap.BackColor1"/>)</param>
--- <param name="BackColor2">secondary background color (RGB value, <see cref="Cap.BackColor2"/>)</param>
--- <param name="ForeColor">Primary fore color (RGB value, <see cref="Cap.ForeColor"/>)</param>
--- <param name="MainPicture">Description of cap primary picture (<see cref="Cap.MainPicture"/>)</param>
--- <param name="TopText">Text of top side of cap (<see cref="Cap.TopText"/>)</param>
--- <param name="SideText">Text of side of cap (<see cref="Cap.SideText"/>)</param>
--- <param name="BottomText">text of bottom side of cap (<see cref="Cap.BottomText"/>)</param>
--- <param name="MaterialID">ID of material cap is made from (<see cref="Cap.MaterialID"/>)</param>
--- <param name="Surface">Type of cap surface - G for glossy, M for matting (<see cref="Cap.Surface"/>)</param>
--- <param name="Size">Primary dimension of cap in millimeters (<see cref="Cap.Size"/>)</param>
--- <param name="Size2">Secondary dimension of cap in millimeters (<see cref="Cap.Size2"/>)</param>
--- <param name="Height">Height of cap in millimeters (Z-size, <see cref="Cap.Heigh"/>)</param>
--- <param name="Is3D">Indicates if cap surface contains 3-dimensional drawing or writing (<see cref="Cap.Is3D"/>)</param>
--- <param name="MainText">The yaer cap was found (<see cref="Cap.Year"/>)</param>
--- <param name="CountryCode">ISO 3166 2-letters country code of country cap was found in (<see cref="Cap.CountryCode"/>)</param>
--- <param name="Note">Additional textual note on cap (<see cref="Cap.Note"/>)</param>
--- <param name="CompanyID">ID of company which's product was contained in object (bottle) closed by this cap (<see cref="Cap.CompanyID"/>)</param>
--- <param name="ProductID">ID of product contained in object (bottle) closed by this cap (<see cref="Cap.ProductID"/>)</param>
--- <param name="ProductTypeID">Type of product contained in object (bottle) closed by this cap (<see cref="Cap.ProductTypeID"/>)</param>
--- <param name="StorageID">ID of storage cap is stored in (<see cref="Cap.StorageID"/>)</param>
--- <param name="ForeColor2">Secondary foreground color (RGB value, <see cref="Cap.ForeColor2"/>)</param>
--- <param name="PictureType">Indicates type of the most complicated image shown on ca - G for geometry, L for logo, D for drawing, P for photo (<see cref="Cap.PictureType"/>)</param>
--- <param name="HasBottom">Indicates if there is something notable at cap bottom side (<see cref="Cap.HasBottom"/>)</param>
--- <param name="HasSide">Indicates if there is something notable at cap side (<see cref="Cap.HasSide"/>)</param>
--- <param name="AnotherPictures">Describes all but main pictures show on cap (<see cref="Cap.AnotherPictures"/>)</param>
--- <param name="CategoryIDs">IDs of categories this cap is flagged with (<see cref="Category.CategoryID"/>)</param>
--- <param name="Keywords">Keywords this cap is tagged with (<see cref="Keyword.Keyword"/>)</param>
--- <param name="CountryOfOrigin">ISO 3166 2-letters code of country the cap originates from (<see cref="Cap.CountryOfOrigin"/>)</param>
--- <param name="IsDrink">Indicates whete cap cosed object (bottle) with drink (<see cref="Cap.IsDrink"/>)</param>
--- <param name="State">Indicates state of cap from 1 best to 5 worst (<see cref="Cap.State"/>)</param>
--- <param name="TargetID">ID of object type (e.g. bottle) tha cap was used on (<see cref="Cap.TargetID"/>)</param>
--- <param name="IsAlcoholic">When <paramref name="IsDrink"/> is true indicate sif the tring was alcoholic (<see cref="Cap.IsAlcoholic"/>)</param>
--- <param name="CapSignIDs">IDs of signs the cap is marked with (<see cref="CapSign.CapSignID"/>)</param>
--- <returns>A resultset containing maximally 100 rows from the <see cref="Cap"/> table most similar to cap described by procedure parameters</returns>
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