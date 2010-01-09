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
