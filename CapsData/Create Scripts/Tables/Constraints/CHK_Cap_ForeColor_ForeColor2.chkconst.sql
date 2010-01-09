ALTER TABLE [dbo].[Cap]
    ADD CONSTRAINT [CHK_Cap_ForeColor_ForeColor2] CHECK (case when [ForeColor] IS NULL AND [ForeColor2] IS NOT NULL then (0) else (1) end<>(0));


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'If ForeColor2 is set ForeColor must be set as well', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cap', @level2type = N'CONSTRAINT', @level2name = N'CHK_Cap_ForeColor_ForeColor2';

