CREATE TABLE [dbo].[CapType] (
    [CapTypeID]   INT            IDENTITY (1, 1) NOT NULL,
    [TypeName]    NVARCHAR (50)  NOT NULL,
    [ShapeID]     INT            NOT NULL,
    [Size]        INT            NOT NULL,
    [Size2]       INT            NULL,
    [Height]      INT            NOT NULL,
    [MaterialID]  INT            NOT NULL,
    [MainTypeID]  INT            NOT NULL,
    [Description] NVARCHAR (MAX) NULL,
    [TargetID]    INT            NULL
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Predefined cap types', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CapType';

