CREATE TABLE [dbo].[SimpleTranslation] (
    [SimpleTranslationID] INT            IDENTITY (1, 1) NOT NULL,
    [CategoryID]          INT            NULL,
    [KeywordID]           INT            NULL,
    [ProductID]           INT            NULL,
    [CompanyID]           INT            NULL,
    [ProductTypeID]       INT            NULL,
    [TargetID]            INT            NULL,
    [MaterialID]          INT            NULL,
    [CapTypeID]           INT            NULL,
    [ShapeID]             INT            NULL,
    [MainTypeID]          INT            NULL,
    [CapSignID]           INT            NULL,
    [StorageID]           INT            NULL,
    [StorageTypeID]       INT            NULL,
    [CapInstanceID]       INT            NULL,
    [Culture]             VARCHAR (15)   NOT NULL,
    [Name]                NVARCHAR (50)  NULL,
    [Description]         NVARCHAR (MAX) NULL
);

