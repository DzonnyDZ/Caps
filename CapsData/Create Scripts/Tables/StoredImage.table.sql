CREATE TABLE [dbo].[StoredImage] (
    [StoredImageID] INT             IDENTITY (1, 1) NOT NULL,
    [FileName]      NVARCHAR (1024) NOT NULL,
    [MIME]          NVARCHAR (100)  NOT NULL,
    [Size]          INT             NOT NULL,
    [Width]         INT             NOT NULL,
    [Height]        INT             NOT NULL,
    [ImageID]       INT             NULL,
    [CapSignID]     INT             NULL,
    [CapTypeID]     INT             NULL,
    [MainTypeID]    INT             NULL,
    [ShapeID]       INT             NULL,
    [StorageID]     INT             NULL,
    [Data]          VARBINARY (MAX) NOT NULL
);



