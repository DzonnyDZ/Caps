ALTER TABLE [dbo].[SimpleTranslation]
    ADD CONSTRAINT [FK_SimpleTranslation_Shape] FOREIGN KEY ([ShapeID]) REFERENCES [dbo].[Shape] ([ShapeID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

