CREATE TRIGGER [dbo].Storage_Instead_Upd
    ON  [dbo].Storage 
    INSTEAD OF UPDATE
AS 
BEGIN
    SET NOCOUNT ON;
	UPDATE [dbo].Storage	
    SET	StorageNumber = i.StorageNumber,
        [Description] = dbo.EmptyStrToNull(i.[Description]),
        StorageTypeid = i.StorageTypeid,
        HasCaps = i.HasCaps,
        ParentStorageID = i.ParentStorageID
     FROM INSERTED AS i
     WHERE Storage.StorageID = i.StorageID;
END