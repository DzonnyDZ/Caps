ALTER TABLE [dbo].[SimpleTranslation]
    ADD CONSTRAINT [FK_SimpleTranslation_Storage] FOREIGN KEY ([StorageID]) REFERENCES [dbo].[Storage] ([StorageID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

