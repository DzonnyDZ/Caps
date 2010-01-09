ALTER TABLE [dbo].[SimpleTranslation]
    ADD CONSTRAINT [FK_SimpleTranslation_CapInstance] FOREIGN KEY ([CapInstanceID]) REFERENCES [dbo].[CapInstance] ([CapInstanceID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

