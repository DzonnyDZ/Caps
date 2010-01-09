ALTER TABLE [dbo].[Shape]
    ADD CONSTRAINT [CHK_Shape_SizeName] CHECK ([Size1Name]<>[Size2Name]);

