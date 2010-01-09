


CREATE TRIGGER [dbo].productype_Instead_Upd
   ON  [dbo].producttype 
   instead of update
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	  UPDATE [dbo].producttype	
   SET	
   producttypename=dbo.EmptyStrToNull(i.producttypename)   ,
				 
      Description=dbo.EmptyStrToNull(i.description),
     isdrink=i.isdrink, IsAlcoholic=i.IsAlcoholic

 from inserted	as i
 WHERE producttype.producttypeid=i.producttypeid

					;



			 
END