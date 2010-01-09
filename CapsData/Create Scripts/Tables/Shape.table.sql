CREATE TABLE [dbo].[Shape] (
    [ShapeID]     INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (50)  NOT NULL,
    [Size1Name]   NVARCHAR (50)  NOT NULL,
    [Size2Name]   NVARCHAR (50)  NULL,
    [Description] NVARCHAR (MAX) NULL
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Cap physical shapes', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Shape';

