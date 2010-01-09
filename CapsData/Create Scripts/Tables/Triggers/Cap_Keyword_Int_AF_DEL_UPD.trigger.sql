-- =============================================
-- Author:		Name
-- Create date: 
-- Description:	
-- =============================================
CREATE TRIGGER dbo.Cap_Keyword_Int_AF_DEL_UPD 
   ON  dbo.Cap_Keyword_Int 
   AFTER DELETE, UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	delete from dbo.Keyword where (SELECT COUNT(*) FROM dbo.Cap_Keyword_Int CKI WHERE CKI.KeywordID=Keyword.KeywordID)=0;
END
