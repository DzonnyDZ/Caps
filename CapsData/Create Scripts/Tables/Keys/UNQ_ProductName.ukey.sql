﻿ALTER TABLE [dbo].[Product]
    ADD CONSTRAINT [UNQ_ProductName] UNIQUE NONCLUSTERED ([CompanyID] ASC, [ProductName] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);
