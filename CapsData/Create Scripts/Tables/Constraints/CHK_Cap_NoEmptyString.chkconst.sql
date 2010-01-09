ALTER TABLE [dbo].[Cap]
    ADD CONSTRAINT [CHK_Cap_NoEmptyString] CHECK ([CapName]<>'' AND [MainText]<>'' AND [SubTitle]<>'' AND [MainPicture]<>'' AND TopText <>'' AND [SideText]<>'' AND [BottomText]<>'' AND [CountryCode]<>'' AND [Note]<>'' AND [AnotherPictures]<>'');

