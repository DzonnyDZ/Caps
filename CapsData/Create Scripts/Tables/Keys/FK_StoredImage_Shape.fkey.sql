ALTER TABLE [dbo].[StoredImage]
    ADD CONSTRAINT [FK_StoredImage_Shape] FOREIGN KEY ([ShapeID]) REFERENCES [dbo].[Shape] ([ShapeID]) ON DELETE CASCADE ON UPDATE NO ACTION;

