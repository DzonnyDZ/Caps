ALTER TABLE [dbo].[Cap_Category_Int]
    ADD CONSTRAINT [FK_Cap_Category_Int_Category] FOREIGN KEY ([CategoryID]) REFERENCES [dbo].[Category] ([CategoryID]) ON DELETE CASCADE ON UPDATE NO ACTION;

