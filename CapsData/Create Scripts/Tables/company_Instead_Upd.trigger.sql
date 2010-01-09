


CREATE TRIGGER [dbo].company_Instead_Upd
   ON  [dbo].company 
   instead of update
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	  UPDATE [dbo].company	
   SET	
   companyName=dbo.EmptyStrToNull(i.companyName),
   
      Description=dbo.EmptyStrToNull(i.description)

 from inserted	as i
 WHERE company.companyid=i.companyid

					;



			 
END