ALTER TABLE [dbo].[Cap]
    ADD CONSTRAINT [CHK_Cap_MainImage_PictureType] CHECK (case when [MainPicture] IS NULL AND [PictureType] IS NOT NULL then (0) when [MainPicture] IS NOT NULL AND [PictureType] IS NULL then (0) else (1) end<>(0));


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'MainImage & PictureType - both set or both NULL', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cap', @level2type = N'CONSTRAINT', @level2name = N'CHK_Cap_MainImage_PictureType';

