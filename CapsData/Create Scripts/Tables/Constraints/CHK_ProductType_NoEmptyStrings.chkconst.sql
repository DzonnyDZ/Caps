ALTER TABLE [dbo].[ProductType]
    ADD CONSTRAINT [CHK_ProductType_NoEmptyStrings] CHECK ([ProductTypeName]<>'' AND [Description]<>'');

