ALTER TABLE [dbo].[Image]
    ADD CONSTRAINT [CHK_Image_NoEmptyStrings] CHECK ([RelativePath]<>'');

