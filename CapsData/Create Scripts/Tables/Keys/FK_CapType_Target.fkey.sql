﻿ALTER TABLE [dbo].[CapType]
    ADD CONSTRAINT [FK_CapType_Target] FOREIGN KEY ([TargetID]) REFERENCES [dbo].[Target] ([TargetID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

