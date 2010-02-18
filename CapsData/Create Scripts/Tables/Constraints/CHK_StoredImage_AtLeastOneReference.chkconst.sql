/*ALTER TABLE [dbo].[StoredImage]
    ADD CONSTRAINT [CHK_StoredImage_AtLeastOneReference] CHECK (case when [ImageID] IS NOT NULL OR [CapSignID] IS NOT NULL OR [CapTypeID] IS NOT NULL OR [MainTypeID] IS NOT NULL OR [ShapeID] IS NOT NULL OR [StorageID] IS NOT NULL then (1) else (0) end=(1));*/

