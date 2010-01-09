ALTER TABLE [dbo].[StoredImage]
    ADD CONSTRAINT [FK_StoredImage_Image] FOREIGN KEY ([ImageID]) REFERENCES [dbo].[Image] ([ImageID]) ON DELETE CASCADE ON UPDATE NO ACTION;

