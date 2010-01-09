ALTER TABLE [dbo].[CapInstance]
    ADD CONSTRAINT [CHK_CapInstance_State] CHECK ([STATE]=(5) OR [STATE]=(4) OR [STATE]=(3) OR [STATE]=(2) OR [STATE]=(1));

