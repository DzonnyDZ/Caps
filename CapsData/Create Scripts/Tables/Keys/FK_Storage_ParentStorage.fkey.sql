﻿ALTER TABLE [dbo].[Storage]
    ADD CONSTRAINT [FK_Storage_ParentStorage] FOREIGN KEY ([ParentStorage]) REFERENCES [dbo].[Storage] ([StorageID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

