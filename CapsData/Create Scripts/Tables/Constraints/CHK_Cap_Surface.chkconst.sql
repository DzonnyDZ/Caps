ALTER TABLE [dbo].[Cap]
    ADD CONSTRAINT [CHK_Cap_Surface] CHECK ([SURFACE]='G' OR [SURFACE]='M');

