ALTER TABLE [dbo].[SimpleTranslation]
    ADD CONSTRAINT [FK_SimpleTranslation_StorageType] FOREIGN KEY ([StorageTypeID]) REFERENCES [dbo].[StorageType] ([StorageTypeID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

