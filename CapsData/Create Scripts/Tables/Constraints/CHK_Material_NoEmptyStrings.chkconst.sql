ALTER TABLE [dbo].[Material]
    ADD CONSTRAINT [CHK_Material_NoEmptyStrings] CHECK ([Name]<>'' AND [Description]<>'');

