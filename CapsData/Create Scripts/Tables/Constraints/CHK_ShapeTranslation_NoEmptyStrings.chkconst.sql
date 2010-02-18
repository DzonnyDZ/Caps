ALTER TABLE [dbo].[ShapeTranslation]
    ADD CONSTRAINT [CHK_ShapeTranslation_NoEmptyStrings] CHECK ([Culture]<>'' AND [Size1Name]<>'' AND [Size2Name]<>'' AND [Description]<>'');

