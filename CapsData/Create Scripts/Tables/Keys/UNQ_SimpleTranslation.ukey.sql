ALTER TABLE [dbo].[SimpleTranslation]
    ADD CONSTRAINT [UNQ_SimpleTranslation] UNIQUE NONCLUSTERED ([CategoryID] ASC, [KeywordID] ASC, [ProductID] ASC, [ProductTypeID] ASC, [TargetID] ASC, [MaterialID] ASC, [CapTypeID] ASC, [ShapeID] ASC, [MainTypeID] ASC, [CapSignID] ASC, [StorageID] ASC, [StorageTypeID] ASC, [CompanyID] ASC, [Culture] ASC, [CapInstanceID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);

