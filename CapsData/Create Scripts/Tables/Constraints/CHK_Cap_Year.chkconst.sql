ALTER TABLE [dbo].[Cap]
    ADD CONSTRAINT [CHK_Cap_Year] CHECK ([Year]>(0) AND [Year]<=(9999));

