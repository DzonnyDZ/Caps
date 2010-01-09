ALTER TABLE [dbo].[Product]
    ADD CONSTRAINT [CHK_Product_NoEmptyStrings] CHECK ([ProductName]<>'' AND [Description]<>'');

