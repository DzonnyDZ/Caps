ALTER TABLE [dbo].[Cap]
    ADD CONSTRAINT [CK_Cap_CountryOfOrigin] CHECK ([CountryOfOrigin] like '[A-Z][A-Z]');

