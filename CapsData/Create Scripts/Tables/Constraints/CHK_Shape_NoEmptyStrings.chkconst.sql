ALTER TABLE [dbo].[Shape]
    ADD CONSTRAINT [CHK_Shape_NoEmptyStrings] CHECK ([Name]<>'' AND [Size1Name]<>'' AND [Size2Name]<>'' AND [Description]<>'');

