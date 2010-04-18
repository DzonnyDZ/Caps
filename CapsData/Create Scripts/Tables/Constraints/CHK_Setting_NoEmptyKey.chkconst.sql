ALTER TABLE [dbo].[Settings]
    ADD CONSTRAINT [CHK_Setting_NoEmptyKey] CHECK ([Key]<>'');

