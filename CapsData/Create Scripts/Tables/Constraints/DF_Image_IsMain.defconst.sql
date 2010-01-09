ALTER TABLE [dbo].[Image]
    ADD CONSTRAINT [DF_Image_IsMain] DEFAULT ((0)) FOR [IsMain];

