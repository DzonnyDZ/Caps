ALTER TABLE [dbo].[Company]
    ADD CONSTRAINT [CHK_Company_NoEmptyString] CHECK ([CompanyName]<>'' AND [Description]<>'');

