
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