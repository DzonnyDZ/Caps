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
