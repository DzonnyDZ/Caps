ALTER TABLE [dbo].[CapType]
    ADD CONSTRAINT [CHK_CapType_Size] CHECK ([Size]>=(0));

