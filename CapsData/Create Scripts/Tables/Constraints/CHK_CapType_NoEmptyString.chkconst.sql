ALTER TABLE [dbo].[CapType]
    ADD CONSTRAINT [CHK_CapType_NoEmptyString] CHECK ([TypeName]<>'' AND [Description]<>'');

