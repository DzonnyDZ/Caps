ALTER TABLE [dbo].[CapType]
    ADD CONSTRAINT [FK_CapType_Material] FOREIGN KEY ([MaterialID]) REFERENCES [dbo].[Material] ([MaterialID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

