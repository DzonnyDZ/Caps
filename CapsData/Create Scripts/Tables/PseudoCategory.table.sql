CREATE TABLE [dbo].[PseudoCategory] (
    [PseudoCategoryID] INT             IDENTITY (1, 1) NOT NULL,
    [Name]             NVARCHAR (50)   NOT NULL,
    [Description]      NVARCHAR (MAX)  NULL,
    [Condition]        NVARCHAR (1024) NOT NULL
);



