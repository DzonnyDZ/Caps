﻿ALTER TABLE [dbo].[Target]
    ADD CONSTRAINT [UNQ_Target] UNIQUE NONCLUSTERED ([Name] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);

