CREATE TRIGGER [dbo].[Cap_Instead_Upd] 
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
