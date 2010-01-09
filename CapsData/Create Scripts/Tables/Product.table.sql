CREATE TABLE [dbo].[Product] (
    [ProductID]     INT            IDENTITY (1, 1) NOT NULL,
    [ProductName]   NVARCHAR (50)  NOT NULL,
    [CompanyID]     INT            NULL,
    [ProductTypeID] INT            NULL,
    [Description]   NVARCHAR (MAX) NULL
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Predefined cap products', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Product';

