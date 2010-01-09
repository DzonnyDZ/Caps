ALTER TABLE [dbo].[Storage]
    ADD CONSTRAINT [DF_Storage_HasCaps] DEFAULT ((1)) FOR [HasCaps];

