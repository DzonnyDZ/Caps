ALTER TABLE [dbo].[Cap]
    ADD CONSTRAINT [CHK_Cap_PictureType] CHECK ([PictureType]='P' OR [PictureType]='D' OR [PictureType]='L' OR [PictureType]='G');


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cap', @level2type = N'CONSTRAINT', @level2name = N'CHK_Cap_PictureType';

