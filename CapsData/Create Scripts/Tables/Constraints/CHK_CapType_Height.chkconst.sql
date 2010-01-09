ALTER TABLE [dbo].[CapType]
    ADD CONSTRAINT [CHK_CapType_Height] CHECK ([Height]>=(0));

