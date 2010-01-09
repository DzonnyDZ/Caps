﻿ALTER TABLE [dbo].[CapType]
    ADD CONSTRAINT [UNQ_CapType_Name] UNIQUE NONCLUSTERED ([TypeName] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);

