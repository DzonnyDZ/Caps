﻿ALTER TABLE [dbo].[ShapeTranslation]
    ADD CONSTRAINT [UNQ_ShapeTranslation] UNIQUE NONCLUSTERED ([ShapeID] ASC, [Culture] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF) ON [PRIMARY];
