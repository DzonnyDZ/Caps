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