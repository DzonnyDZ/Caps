ALTER TABLE [dbo].[StoredImage]
    ADD CONSTRAINT [CHK_StoredImage_OneReference] CHECK ((((((case when [ImageID] IS NULL then (0) else (1) end+case when [CapSignID] IS NULL then (0) else (1) end)+case when [CapTypeID] IS NULL then (0) else (1) end)+case when [MainTypeID] IS NULL then (0) else (1) end)+case when [ShapeID] IS NULL then (0) else (1) end)+case when [StorageID] IS NULL then (0) else (1) end)=(1));

