


CREATE TRIGGER [dbo].Keyword_Instead_Upd
   ON  [dbo].Keyword 
   instead of update
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	  UPDATE [dbo].Keyword	
   SET	
   keyword=dbo.EmptyStrToNull(i.keyword)
   
      --Description=dbo.EmptyStrToNull(i.description)
     

 from inserted	as i
 WHERE Keyword.Keywordid=i.Keywordid

					;



			 
END


