ALTER TABLE [dbo].[SimpleTranslation]
    ADD CONSTRAINT [UNQ_SimpleTranslation] UNIQUE NONCLUSTERED ([CategoryID] ASC, [KeywordID] ASC, [ProductID] ASC, [ProductTypeID] ASC, [TargetID] ASC, [MaterialID] ASC, [CapTypeID] ASC, [MainTypeID] ASC, [CapSignID] ASC, [StorageID] ASC, [StorageTypeID] ASC, [CompanyID] ASC, [CapInstanceID] ASC, [Culture] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF) ON [PRIMARY];



