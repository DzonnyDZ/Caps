ALTER TABLE [dbo].[SimpleTranslation]
    ADD CONSTRAINT [FK_SimpleTranslation_Target] FOREIGN KEY ([TargetID]) REFERENCES [dbo].[Target] ([TargetID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

