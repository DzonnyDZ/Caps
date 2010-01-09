ALTER TABLE [dbo].[CapInstance]
    ADD CONSTRAINT [FK_CapInstance_Storage] FOREIGN KEY ([StorageID]) REFERENCES [dbo].[Storage] ([StorageID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

