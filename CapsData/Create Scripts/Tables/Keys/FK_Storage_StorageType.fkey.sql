ALTER TABLE [dbo].[Storage]
    ADD CONSTRAINT [FK_Storage_StorageType] FOREIGN KEY ([StorageTypeID]) REFERENCES [dbo].[StorageType] ([StorageTypeID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

