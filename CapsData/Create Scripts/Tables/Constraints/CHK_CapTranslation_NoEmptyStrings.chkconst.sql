ALTER TABLE [dbo].[CapTranslation]
    ADD CONSTRAINT [CHK_CapTranslation_NoEmptyStrings] CHECK ([Culture]<>'' AND [CapName]<>'' AND [MainPicture]<>'' AND [Note]<>'' AND [AnotherPictures]<>'');

