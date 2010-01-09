ALTER TABLE [dbo].[Cap]
    ADD CONSTRAINT [CHK_Cap_Size] CHECK ([Size]>=(0));

