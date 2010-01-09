CREATE TABLE [dbo].[Image] (
    [ImageID]      INT            IDENTITY (1, 1) NOT NULL,
    [RelativePath] NVARCHAR (256) NOT NULL,
    [CapID]        INT            NOT NULL,
    [IsMain]       BIT            NOT NULL
);



