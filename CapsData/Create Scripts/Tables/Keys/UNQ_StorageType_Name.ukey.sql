﻿ALTER TABLE [dbo].[StorageType]
    ADD CONSTRAINT [UNQ_StorageType_Name] UNIQUE NONCLUSTERED ([Name] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);
