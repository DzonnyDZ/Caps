ALTER TABLE [dbo].[SimpleTranslation]
    ADD CONSTRAINT [FK_SimpleTranslation_Material] FOREIGN KEY ([MaterialID]) REFERENCES [dbo].[Material] ([MaterialID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

