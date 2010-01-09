ALTER TABLE [dbo].[ISO 3166-1]
    ADD CONSTRAINT [CHK_ISO 3166-1_Alpha-2] CHECK ([Alpha-2] like '[A-Z][A-Z]');

