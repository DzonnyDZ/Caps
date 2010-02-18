ALTER TABLE [dbo].[ShapeTranslation]
    ADD CONSTRAINT [FK_ShapeTranslation_Shape] FOREIGN KEY ([ShapeID]) REFERENCES [dbo].[Shape] ([ShapeID]) ON DELETE CASCADE ON UPDATE NO ACTION;

