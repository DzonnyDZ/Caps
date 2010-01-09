﻿ALTER TABLE [dbo].[StoredImage]
    ADD CONSTRAINT [CHK_StoredImage_OnlyOneReference] CHECK (case when [ImageID] IS NOT NULL AND [CapSignID] IS NULL AND [CapTypeID] IS NULL AND [MainTypeID] IS NULL AND [ShapeID] IS NULL AND [StorageID] IS NULL OR [ImageID] IS NULL AND [CapSignID] IS NOT NULL AND [CapTypeID] IS NULL AND [MainTypeID] IS NULL AND [ShapeID] IS NULL AND [StorageID] IS NULL OR [ImageID] IS NULL AND [CapSignID] IS NULL AND [CapTypeID] IS NOT NULL AND [MainTypeID] IS NULL AND [ShapeID] IS NULL AND [StorageID] IS NULL OR [ImageID] IS NULL AND [CapSignID] IS NULL AND [CapTypeID] IS NULL AND [MainTypeID] IS NOT NULL AND [ShapeID] IS NULL AND [StorageID] IS NULL OR [ImageID] IS NULL AND [CapSignID] IS NULL AND [CapTypeID] IS NULL AND [MainTypeID] IS NULL AND [ShapeID] IS NOT NULL AND [StorageID] IS NULL OR [ImageID] IS NULL AND [CapSignID] IS NULL AND [CapTypeID] IS NULL AND [MainTypeID] IS NULL AND [ShapeID] IS NULL AND [StorageID] IS NOT NULL then (1) else (0) end=(1));

