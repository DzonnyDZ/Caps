ALTER TABLE [dbo].[Cap]
    ADD CONSTRAINT [CHK_Cap_NoEmptyString] CHECK ([CapName]<>'' AND [MainText]<>'' AND [SubTitle]<>'' AND [MainPicture]<>'' AND [toptext]<>'' AND [sidetext]<>'' AND [bottomtext]<>'' AND [countrycode]<>'' AND [note]<>'' AND [anotherpictures]<>'');

