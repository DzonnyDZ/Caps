﻿ALTER TABLE [dbo].[CapTranslation]
    ADD CONSTRAINT [UNQ_trnCap] UNIQUE NONCLUSTERED ([CapID] ASC, [Culture] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);
