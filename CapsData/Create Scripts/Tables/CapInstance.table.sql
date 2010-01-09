CREATE TABLE [dbo].[CapInstance] (
    [CapInstanceID] INT            IDENTITY (1, 1) NOT NULL,
    [CapID]         INT            NOT NULL,
    [StorageID]     INT            NOT NULL,
    [State]         SMALLINT       NOT NULL,
    [Year]          INT            NULL,
    [CountryCode]   CHAR (2)       NULL,
    [DateCreated]   DATETIME       NOT NULL,
    [Note]          NVARCHAR (MAX) NULL
);



