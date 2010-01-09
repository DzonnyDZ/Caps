ALTER TABLE [dbo].[SimpleTranslation]
    ADD CONSTRAINT [FK_SimpleTranslation_CapType] FOREIGN KEY ([CapTypeID]) REFERENCES [dbo].[CapType] ([CapTypeID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

