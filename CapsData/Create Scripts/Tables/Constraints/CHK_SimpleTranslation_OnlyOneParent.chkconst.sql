﻿ALTER TABLE [dbo].[SimpleTranslation]
    ADD CONSTRAINT [CHK_SimpleTranslation_OnlyOneParent] CHECK (((((((((((((case when [CategoryID] IS NULL then (0) else (1) end+case when [KeywordID] IS NULL then (0) else (1) end)+case when [ProductID] IS NULL then (0) else (1) end)+case when [CompanyID] IS NULL then (0) else (1) end)+case when [ProductTypeID] IS NULL then (0) else (1) end)+case when [TargetID] IS NULL then (0) else (1) end)+case when [MaterialID] IS NULL then (0) else (1) end)+case when [CapTypeID] IS NULL then (0) else (1) end)+case when [MainTypeID] IS NULL then (0) else (1) end)+case when [CapSignID] IS NULL then (0) else (1) end)+case when [StorageID] IS NULL then (0) else (1) end)+case when [StorageTypeID] IS NULL then (0) else (1) end)+case when [CapInstanceID] IS NULL then (0) else (1) end)=(1));



