ALTER TABLE [dbo].[PseudoCategory]
    ADD CONSTRAINT [CHK_PseudoCategory_NoEmptyStrings] CHECK ([Name]<>'' AND [Description]<>'' AND [Condition]<>'');

