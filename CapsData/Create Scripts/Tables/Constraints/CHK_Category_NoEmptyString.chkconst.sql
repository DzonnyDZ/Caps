ALTER TABLE [dbo].[Category]
    ADD CONSTRAINT [CHK_Category_NoEmptyString] CHECK ([CategoryName]<>'' AND [Description]<>'');

