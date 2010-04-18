CREATE TABLE [dbo].[Settings] (
    [SettingID] INT            IDENTITY (1, 1) NOT NULL,
    [Key]       NVARCHAR (255) NOT NULL,
    [Value]     NVARCHAR (MAX) NULL
);

