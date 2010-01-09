ALTER TABLE [dbo].[ProductType]
    ADD CONSTRAINT [CHK_ProductType_IsDrinkIsAlcoholic] CHECK (case when [IsDrink] IS NULL then case when [IsAlcoholic] IS NULL then (1) else (0) end when [IsDrink]=(0) then case when [IsAlcoholic] IS NULL OR [IsAlcoholic]=(1) then (0) else (1) end else (1) end=(1));


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'IsDrink IS NULL => IsAlcoholic IS NULL; IsDrink = 0 => IsAlcoholic = 0', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ProductType', @level2type = N'CONSTRAINT', @level2name = N'CHK_ProductType_IsDrinkIsAlcoholic';

