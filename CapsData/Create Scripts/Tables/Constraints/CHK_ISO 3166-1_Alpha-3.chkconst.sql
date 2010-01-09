ALTER TABLE [dbo].[ISO 3166-1]
    ADD CONSTRAINT [CHK_ISO 3166-1_Alpha-3] CHECK ([Alpha-3] like '[A-Z][A-Z][A-Z]');

