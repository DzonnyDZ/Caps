ALTER TABLE [dbo].[CapInstance]
    ADD CONSTRAINT [CHK_CapInstance_State] CHECK ([State]=(5) OR [State]=(4) OR [State]=(3) OR [State]=(2) OR [State]=(1));

