ALTER TABLE [dbo].[SimpleTranslation]
    ADD CONSTRAINT [FK_SimpleTranslation_MainType] FOREIGN KEY ([MainTypeID]) REFERENCES [dbo].[MainType] ([MainTypeID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

