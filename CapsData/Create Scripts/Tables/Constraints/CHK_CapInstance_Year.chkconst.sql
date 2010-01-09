ALTER TABLE [dbo].[CapInstance]
    ADD CONSTRAINT [CHK_CapInstance_Year] CHECK ([Year]>(0) AND [Year]<=(9999));

