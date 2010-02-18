CREATE TABLE [dbo].[ShapeTranslation] (
    [ShapeTranslationID] INT            IDENTITY (1, 1) NOT NULL,
    [ShapeID]            INT            NOT NULL,
    [Culture]            VARCHAR (15)   NOT NULL,
    [Name]               NVARCHAR (50)  NULL,
    [Size1Name]          NVARCHAR (50)  NULL,
    [Size2Name]          NVARCHAR (50)  NULL,
    [Description]        NVARCHAR (MAX) NULL
);

