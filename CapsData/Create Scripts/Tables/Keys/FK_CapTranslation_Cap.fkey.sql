ALTER TABLE [dbo].[CapTranslation]
    ADD CONSTRAINT [FK_CapTranslation_Cap] FOREIGN KEY ([CapID]) REFERENCES [dbo].[Cap] ([CapID]) ON DELETE CASCADE ON UPDATE NO ACTION;

