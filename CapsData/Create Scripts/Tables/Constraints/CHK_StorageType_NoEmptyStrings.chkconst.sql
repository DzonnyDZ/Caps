ALTER TABLE [dbo].[StorageType]
    ADD CONSTRAINT [CHK_StorageType_NoEmptyStrings] CHECK ([Name]<>'' AND [Description]<>'');

