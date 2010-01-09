ALTER TABLE [dbo].[CapSign]
    ADD CONSTRAINT [CHK_CapSign_NoEmptyStrings] CHECK ([Name]<>'' AND [Description]<>'');

