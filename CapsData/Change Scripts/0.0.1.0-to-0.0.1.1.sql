﻿ALTER TRIGGER [dbo].[Cap_Instead_Ins] 
   ON  dbo.Cap 
   instead of INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
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
  FROM inserted	 ;

--select * from dbo.cap where capid=scope_identity();

END;

GO

-------------------------------------------------------------------
ALTER FUNCTION GetDatabaseVersion ()
RETURNS nvarchar(50)
AS
BEGIN
declare @dbGuid nvarchar(38) = '{DAFDAE3F-2F0A-4359-81D6-50BA394D72D9}';
declare @dbVersion nvarchar(11) = '0.0.2.0';
return @dbGuid + @dbversion;

END;
GO