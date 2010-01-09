ALTER TABLE [dbo].[StoredImage]
    ADD CONSTRAINT [CHK_StoredImage_NoEmptyStrings] CHECK ([FileName]<>'' AND [MIME]<>'');

