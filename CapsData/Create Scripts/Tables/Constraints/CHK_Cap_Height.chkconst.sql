ALTER TABLE [dbo].[Cap]
    ADD CONSTRAINT [CHK_Cap_Height] CHECK ([Height]>=(0));

