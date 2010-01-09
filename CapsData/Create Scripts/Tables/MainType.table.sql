CREATE TABLE [dbo].[MainType] (
    [MainTypeID]  INT            IDENTITY (1, 1) NOT NULL,
    [TypeName]    NVARCHAR (50)  NOT NULL,
    [Description] NVARCHAR (MAX) NULL
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'General cap types', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MainType';

