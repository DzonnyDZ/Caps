ALTER TABLE [dbo].[StoredImage]
    ADD CONSTRAINT [CHK_StoredImage_OneReference] CHECK ((((((case when [ImageID] IS NULL then (1) else (0) end+case when [CapSignID] IS NULL then (1) else (0) end)+case when [CapTypeID] IS NULL then (1) else (0) end)+case when [MainTypeID] IS NULL then (1) else (0) end)+case when [ShapeID] IS NULL then (1) else (0) end)+case when [StorageID] IS NULL then (1) else (0) end)=(1));

