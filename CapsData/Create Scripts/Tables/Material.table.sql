CREATE TABLE [dbo].[Material] (
    [MaterialID]  INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (50)  NOT NULL,
    [Description] NVARCHAR (MAX) NULL
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Cap materials', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Material';

