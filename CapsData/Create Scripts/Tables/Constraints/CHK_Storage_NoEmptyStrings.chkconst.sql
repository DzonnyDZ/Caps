ALTER TABLE [dbo].[Storage]
    ADD CONSTRAINT [CHK_Storage_NoEmptyStrings] CHECK ([Description]<>'' AND [storagenumber]<>'');

