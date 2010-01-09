ALTER TABLE [dbo].[StoredImage]
    ADD CONSTRAINT [FK_StoredImage_CapType] FOREIGN KEY ([CapTypeID]) REFERENCES [dbo].[CapType] ([CapTypeID]) ON DELETE CASCADE ON UPDATE NO ACTION;

