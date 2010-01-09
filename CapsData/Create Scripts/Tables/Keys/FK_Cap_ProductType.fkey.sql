ALTER TABLE [dbo].[Cap]
    ADD CONSTRAINT [FK_Cap_ProductType] FOREIGN KEY ([ProductTypeID]) REFERENCES [dbo].[ProductType] ([ProductTypeID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

