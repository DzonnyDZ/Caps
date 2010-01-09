CREATE TABLE [dbo].[Keyword] (
    [KeywordID] INT           IDENTITY (1, 1) NOT NULL,
    [Keyword]   NVARCHAR (50) NOT NULL
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Automatically filled cap keywords', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Keyword';

