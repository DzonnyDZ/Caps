﻿ALTER TABLE [dbo].[PseudoCategory]
    ADD CONSTRAINT [PK_PseudoCategory] PRIMARY KEY CLUSTERED ([PseudoCategoryID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);
