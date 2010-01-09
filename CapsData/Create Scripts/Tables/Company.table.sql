CREATE TABLE [dbo].[Company] (
    [CompanyID]   INT            IDENTITY (1, 1) NOT NULL,
    [CompanyName] NVARCHAR (50)  NOT NULL,
    [Description] NVARCHAR (MAX) NULL
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Companies', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Company';

