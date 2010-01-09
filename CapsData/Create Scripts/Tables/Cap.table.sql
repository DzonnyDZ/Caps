CREATE TABLE [dbo].[Cap] (
    [CapID]           INT            IDENTITY (1, 1) NOT NULL,
    [CapTypeID]       INT            NULL,
    [MainTypeID]      INT            NOT NULL,
    [ShapeID]         INT            NOT NULL,
    [CapName]         NVARCHAR (255) NOT NULL,
    [MainText]        NVARCHAR (255) NULL,
    [SubTitle]        NVARCHAR (255) NULL,
    [BackColor1]      INT            NOT NULL,
    [BackColor2]      INT            NULL,
    [ForeColor]       INT            NULL,
    [MainPicture]     NVARCHAR (255) NULL,
    [TopText]         NVARCHAR (MAX) NULL,
    [SideText]        NVARCHAR (MAX) NULL,
    [BottomText]      NVARCHAR (MAX) NULL,
    [MaterialID]      INT            NOT NULL,
    [Surface]         CHAR (1)       NOT NULL,
    [Size]            INT            NOT NULL,
    [Size2]           INT            NULL,
    [Height]          INT            NOT NULL,
    [Is3D]            BIT            NOT NULL,
    [Year]            INT            NULL,
    [CountryCode]     CHAR (2)       COLLATE Latin1_General_BIN NULL,
    [DateCreated]     DATETIME       NOT NULL,
    [Note]            NVARCHAR (MAX) NULL,
    [CompanyID]       INT            NULL,
    [ProductID]       INT            NULL,
    [ProductTypeID]   INT            NULL,
    [StorageID]       INT            NOT NULL,
    [ForeColor2]      INT            NULL,
    [PictureType]     CHAR (1)       NULL,
    [HasBottom]       BIT            NOT NULL,
    [HasSide]         BIT            NOT NULL,
    [AnotherPictures] NVARCHAR (MAX) NULL,
    [CountryOfOrigin] CHAR (2)       COLLATE Latin1_General_BIN NULL,
    [IsDrink]         BIT            NULL,
    [State]           SMALLINT       NOT NULL,
    [TargetID]        INT            NULL,
    [IsAlcoholic]     BIT            NULL
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cap';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Unique id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cap', @level2type = N'COLUMN', @level2name = N'CapID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Reference to standartized capy type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cap', @level2type = N'COLUMN', @level2name = N'CapTypeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Reference to general cap type', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cap', @level2type = N'COLUMN', @level2name = N'MainTypeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Reference to cap shape', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cap', @level2type = N'COLUMN', @level2name = N'ShapeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Name of cap (usually sma as MainText or MainText + SubTitle or MainPicture)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cap', @level2type = N'COLUMN', @level2name = N'CapName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Main writing on cap (if any)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cap', @level2type = N'COLUMN', @level2name = N'MainText';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Second most visible writng on cap (if any)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cap', @level2type = N'COLUMN', @level2name = N'SubTitle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'First cap bacground color', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cap', @level2type = N'COLUMN', @level2name = N'BackColor1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Secondary background color of cap (if any)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cap', @level2type = N'COLUMN', @level2name = N'BackColor2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Main cap fore color (if it has any foreground)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cap', @level2type = N'COLUMN', @level2name = N'ForeColor';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Description of cap main pictre (if any)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cap', @level2type = N'COLUMN', @level2name = N'MainPicture';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Text of cap top side', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cap', @level2type = N'COLUMN', @level2name = N'TopText';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Text of cap sides', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cap', @level2type = N'COLUMN', @level2name = N'SideText';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Text of cap bottom side', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cap', @level2type = N'COLUMN', @level2name = N'BottomText';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Reference to cap material it is mainly made of', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cap', @level2type = N'COLUMN', @level2name = N'MaterialID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Cap surface, M for matting, G for glossy', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cap', @level2type = N'COLUMN', @level2name = N'Surface';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Cap main size in mm', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cap', @level2type = N'COLUMN', @level2name = N'Size';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Cap seconray size in mm (if shape defines it)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cap', @level2type = N'COLUMN', @level2name = N'Size2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Cap height in mm', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cap', @level2type = N'COLUMN', @level2name = N'Height';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'If cap surface i.e. writing is 3D', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cap', @level2type = N'COLUMN', @level2name = N'Is3D';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Year when cap was found, if known', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cap', @level2type = N'COLUMN', @level2name = N'Year';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ISO2 counry code of country where cap was found, if any', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cap', @level2type = N'COLUMN', @level2name = N'CountryCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'When DB record was created', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cap', @level2type = N'COLUMN', @level2name = N'DateCreated';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Any note', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cap', @level2type = N'COLUMN', @level2name = N'Note';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Geometry < Logo < Drawing < Photo (biggest applicable)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cap', @level2type = N'COLUMN', @level2name = N'PictureType';

