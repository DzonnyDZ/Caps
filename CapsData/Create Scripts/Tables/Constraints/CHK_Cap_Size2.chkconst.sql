ALTER TABLE [dbo].[Cap]
    ADD CONSTRAINT [CHK_Cap_Size2] CHECK ([Size2]>=(0));

