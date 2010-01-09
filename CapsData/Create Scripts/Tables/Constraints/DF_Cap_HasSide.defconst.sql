ALTER TABLE [dbo].[Cap]
    ADD CONSTRAINT [DF_Cap_HasSide] DEFAULT ((0)) FOR [HasSide];

