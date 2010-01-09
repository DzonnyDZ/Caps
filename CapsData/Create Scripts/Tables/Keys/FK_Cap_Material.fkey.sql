ALTER TABLE [dbo].[Cap]
    ADD CONSTRAINT [FK_Cap_Material] FOREIGN KEY ([MaterialID]) REFERENCES [dbo].[Material] ([MaterialID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

