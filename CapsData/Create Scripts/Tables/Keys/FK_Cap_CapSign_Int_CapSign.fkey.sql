ALTER TABLE [dbo].[Cap_CapSign_Int]
    ADD CONSTRAINT [FK_Cap_CapSign_Int_CapSign] FOREIGN KEY ([CapSignID]) REFERENCES [dbo].[CapSign] ([CapSignID]) ON DELETE CASCADE ON UPDATE NO ACTION;

