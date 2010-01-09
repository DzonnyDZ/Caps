ALTER TABLE [dbo].[CapInstance]
    ADD CONSTRAINT [CHK_CapInstance_NoEmptyStrings] CHECK ([Note]<>'');

