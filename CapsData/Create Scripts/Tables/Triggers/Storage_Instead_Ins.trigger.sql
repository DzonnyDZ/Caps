CREATE TRIGGER dbo.Storage_Instead_Ins 
   ON  dbo.Storage
   INSTEAD OF INSERT
AS 
BEGIN
	SET NOCOUNT ON;
    INSERT INTO dbo.Storage (StorageNumber,[Description],StorageTypeID, HasCaps, ParentStorageID)
        OUTPUT INSERTED.*
		SELECT StorageNumber, dbo.EmptyStrToNull([Description]), StorageTypeID, HasCaps, ParentStorageID
  FROM INSERTED;
END
