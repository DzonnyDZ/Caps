CREATE TABLE [dbo].[ProductType] (
    [ProductTypeID]   INT            IDENTITY (1, 1) NOT NULL,
    [ProductTypeName] NVARCHAR (50)  NOT NULL,
    [Description]     NVARCHAR (MAX) NULL,
    [IsDrink]         BIT            NULL,
    [IsAlcoholic]     BIT            NULL
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'General cap product types such as beer or shampoo', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ProductType';

