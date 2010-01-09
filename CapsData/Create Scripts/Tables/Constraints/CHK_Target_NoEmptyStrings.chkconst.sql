ALTER TABLE [dbo].[Target]
    ADD CONSTRAINT [CHK_Target_NoEmptyStrings] CHECK ([Name]<>'' AND [Description]<>'');

