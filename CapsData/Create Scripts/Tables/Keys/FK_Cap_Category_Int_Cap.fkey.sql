ALTER TABLE [dbo].[Cap_Category_Int]
    ADD CONSTRAINT [FK_Cap_Category_Int_Cap] FOREIGN KEY ([CapID]) REFERENCES [dbo].[Cap] ([CapID]) ON DELETE CASCADE ON UPDATE NO ACTION;

