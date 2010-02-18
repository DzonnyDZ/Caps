CREATE TRIGGER [dbo].[SimpleTranslation_Instead_Ins] 
   ON  [dbo].[SimpleTranslation] 
   INSTEAD OF INSERT
AS 
BEGIN
	SET NOCOUNT ON;
	INSERT INTO dbo.SimpleTranslation
		(CategoryID, KeywordID, ProductID, CompanyID, ProductTypeID, TargetID, MaterialID, CapTypeID,  MainTypeID, CapSignID, StorageID, StorageTypeID, CapInstanceID, Culture, Name, [Description])
        OUTPUT inserted.*
		SELECT CategoryID, KeywordID, ProductID, CompanyID, ProductTypeID, TargetID, MaterialID, CapTypeID,  MainTypeID, CapSignID, StorageID, StorageTypeID, CapInstanceID, dbo.EmptyStrToNull(Culture), dbo.EmptyStrToNull(Name), dbo.EmptyStrToNull([Description])
	FROM inserted;
END
