ALTER TABLE [dbo].[SimpleTranslation]
    ADD CONSTRAINT [CHK_SimpleTranslation_NoEmptyStrings] CHECK ([Culture]<>'' AND [Name]<>'' AND [Description]<>'');

