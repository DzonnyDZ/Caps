ALTER TABLE [dbo].[Cap]
    ADD CONSTRAINT [CHK_Cap_AnotherPictures_MainPicture] CHECK (case when [AnotherPictures] IS NOT NULL AND [MainPicture] IS NULL then (0) else (1) end<>(0));


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'AnotherPictures cannot be set when MainPicture is not set', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cap', @level2type = N'CONSTRAINT', @level2name = N'CHK_Cap_AnotherPictures_MainPicture';

