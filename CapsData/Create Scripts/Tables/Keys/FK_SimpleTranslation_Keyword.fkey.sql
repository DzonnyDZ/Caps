ALTER TABLE [dbo].[SimpleTranslation]
    ADD CONSTRAINT [FK_SimpleTranslation_Keyword] FOREIGN KEY ([KeywordID]) REFERENCES [dbo].[Keyword] ([KeywordID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

