ALTER TABLE [dbo].[StoredImage]
    ADD CONSTRAINT [FK_StoredImage_Storage] FOREIGN KEY ([StorageID]) REFERENCES [dbo].[Storage] ([StorageID]) ON DELETE CASCADE ON UPDATE NO ACTION;

