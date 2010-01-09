ALTER TABLE [dbo].[StoredImage]
    ADD CONSTRAINT [FK_StoredImage_CapSign] FOREIGN KEY ([CapSignID]) REFERENCES [dbo].[CapSign] ([CapSignID]) ON DELETE CASCADE ON UPDATE NO ACTION;

