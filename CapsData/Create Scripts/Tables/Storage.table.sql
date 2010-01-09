CREATE TABLE [dbo].[Storage] (
    [StorageID]     INT            IDENTITY (1, 1) NOT NULL,
    [StorageNumber] NVARCHAR (10)  NOT NULL,
    [Description]   NVARCHAR (MAX) NULL,
    [StorageTypeID] INT            NOT NULL,
    [ParentStorage] INT            NULL,
    [HasCaps]       BIT            NOT NULL
);

