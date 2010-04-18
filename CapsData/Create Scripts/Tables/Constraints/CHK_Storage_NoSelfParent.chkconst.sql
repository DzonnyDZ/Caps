ALTER TABLE [dbo].[Storage]
    ADD CONSTRAINT [CHK_Storage_NoSelfParent] CHECK ([StorageID]<>[ParentStorageID]);

