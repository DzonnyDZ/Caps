ALTER TABLE [dbo].[SimpleTranslation]
    ADD CONSTRAINT [FK_SimpleTranslation_ProductType] FOREIGN KEY ([ProductTypeID]) REFERENCES [dbo].[ProductType] ([ProductTypeID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

