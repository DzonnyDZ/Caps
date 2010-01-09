ALTER TABLE [dbo].[SimpleTranslation]
    ADD CONSTRAINT [FK_SimpleTranslation_CapSign] FOREIGN KEY ([CapSignID]) REFERENCES [dbo].[CapSign] ([CapSignID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

