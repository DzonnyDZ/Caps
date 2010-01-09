ALTER TABLE [dbo].[StoredImage]
    ADD CONSTRAINT [FK_StoredImage_MainType] FOREIGN KEY ([MainTypeID]) REFERENCES [dbo].[MainType] ([MainTypeID]) ON DELETE CASCADE ON UPDATE NO ACTION;

