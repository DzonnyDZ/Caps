ALTER TABLE [dbo].[SimpleTranslation]
    ADD CONSTRAINT [FK_SimpleTranslation_Category] FOREIGN KEY ([CategoryID]) REFERENCES [dbo].[Category] ([CategoryID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

