ALTER TABLE [dbo].[Product]
    ADD CONSTRAINT [FK_Product_Company] FOREIGN KEY ([CompanyID]) REFERENCES [dbo].[Company] ([CompanyID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

