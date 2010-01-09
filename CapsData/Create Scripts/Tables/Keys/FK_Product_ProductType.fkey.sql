ALTER TABLE [dbo].[Product]
    ADD CONSTRAINT [FK_Product_ProductType] FOREIGN KEY ([ProductTypeID]) REFERENCES [dbo].[ProductType] ([ProductTypeID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

