CREATE TABLE [dbo].[Romanization] (
    [Character]    NCHAR (1)     COLLATE Latin1_General_100_CS_AS NOT NULL,
    [Romanization] NVARCHAR (10) NOT NULL,
    [Code]         AS            (unicode([Character])) PERSISTED
);



