ALTER TABLE [dbo].[MainType]
    ADD CONSTRAINT [CHK_MainType_NoEmptyStrings] CHECK ([TypeName]<>'' AND [Description]<>'');

