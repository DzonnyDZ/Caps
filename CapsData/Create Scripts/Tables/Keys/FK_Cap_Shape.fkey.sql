﻿ALTER TABLE [dbo].[Cap]
    ADD CONSTRAINT [FK_Cap_Shape] FOREIGN KEY ([ShapeID]) REFERENCES [dbo].[Shape] ([ShapeID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

