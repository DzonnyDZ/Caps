ALTER TABLE [dbo].[Cap]
    ADD CONSTRAINT [CHK_Cap_Surface] CHECK ([Surface]='G' OR [Surface]='M');

