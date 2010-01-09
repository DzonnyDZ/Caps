ALTER TABLE [dbo].[Cap]
    ADD CONSTRAINT [DF_Cap_HasBottom] DEFAULT ((0)) FOR [HasBottom];

