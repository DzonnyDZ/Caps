﻿ALTER TABLE [dbo].[StoredImage]
    ADD CONSTRAINT [PK_StoredImage] PRIMARY KEY CLUSTERED ([StoredImageID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);

