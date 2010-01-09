ALTER TABLE [dbo].[Cap]
    ADD CONSTRAINT [CHK_Cap_IsDrinkIsAlcoholic] CHECK (case when [IsDrink] IS NULL then case when [IsAlcoholic] IS NULL then (1) else (0) end when [IsDrink]=(0) then case when [IsAlcoholic] IS NULL OR [IsAlcoholic]=(1) then (0) else (1) end else (1) end=(1));


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'(IsDrink IS NULL AND IsAlcoholic IS NULL) OR (NOT IsDrink AND NOT IsAlcoholic) OR IsDrink', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cap', @level2type = N'CONSTRAINT', @level2name = N'CHK_Cap_IsDrinkIsAlcoholic';

