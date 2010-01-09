ALTER TABLE [dbo].[CapInstance]
    ADD CONSTRAINT [CHK_CapInstance_CountryCode] CHECK ([CountryCode] like '[A-Z][A-Z]');

