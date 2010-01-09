ALTER TABLE [dbo].[Cap]
    ADD CONSTRAINT [CHK_Cap_Sate] CHECK ([State]=(5) OR [State]=(4) OR [State]=(3) OR [State]=(2) OR [State]=(1));

