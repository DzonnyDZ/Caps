CREATE TABLE [dbo].[CapTranslation] (
    [CapTranslationID] INT            IDENTITY (1, 1) NOT NULL,
    [CapID]            INT            NOT NULL,
    [Culture]          VARCHAR (15)   NOT NULL,
    [CapName]          NVARCHAR (255) NULL,
    [MainPicture]      NVARCHAR (255) NULL,
    [Note]             NVARCHAR (MAX) NULL,
    [AnotherPictures]  NVARCHAR (MAX) NULL
);



