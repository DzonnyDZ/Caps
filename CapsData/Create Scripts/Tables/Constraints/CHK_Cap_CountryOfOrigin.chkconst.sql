ALTER TABLE [dbo].[Cap]
    ADD CONSTRAINT [CHK_Cap_CountryOfOrigin] CHECK ([CountryOfOrigin] like '[A-Z][A-Z]');