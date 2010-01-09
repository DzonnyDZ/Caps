ALTER TABLE [dbo].[Cap]
    ADD CONSTRAINT [CHK_Cap_BottomText_HasBottom] CHECK (case when [BottomText] IS NOT NULL AND [HasBottom]=(0) then (0) else (1) end<>(0));


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'HasBottom must be true when BottomText is set', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cap', @level2type = N'CONSTRAINT', @level2name = N'CHK_Cap_BottomText_HasBottom';

