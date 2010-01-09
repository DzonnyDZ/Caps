ALTER TABLE [dbo].[Cap_Keyword_Int]
    ADD CONSTRAINT [FK_Cap_Keyword_Int_Keyword] FOREIGN KEY ([KeywordID]) REFERENCES [dbo].[Keyword] ([KeywordID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

