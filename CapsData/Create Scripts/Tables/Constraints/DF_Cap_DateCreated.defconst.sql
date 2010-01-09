ALTER TABLE [dbo].[Cap]
    ADD CONSTRAINT [DF_Cap_DateCreated] DEFAULT (getdate()) FOR [DateCreated];

