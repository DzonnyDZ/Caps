ALTER TABLE [dbo].[CapType]
    ADD CONSTRAINT [CHK_CapType_Size2] CHECK ([Size2]>=(0));

