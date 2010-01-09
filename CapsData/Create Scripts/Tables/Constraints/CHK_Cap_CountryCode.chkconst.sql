ALTER TABLE [dbo].[Cap]
    ADD CONSTRAINT [CHK_Cap_CountryCode] CHECK ([CountryCode] like '[A-Z][A-Z]');

