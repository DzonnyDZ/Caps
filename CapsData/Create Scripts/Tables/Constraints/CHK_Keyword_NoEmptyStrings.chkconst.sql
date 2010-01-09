ALTER TABLE [dbo].[Keyword]
    ADD CONSTRAINT [CHK_Keyword_NoEmptyStrings] CHECK ([Keyword]<>'');

