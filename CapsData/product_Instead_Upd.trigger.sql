


CREATE TRIGGER [dbo].product_Instead_Upd
   ON  [dbo].product 
   instead of update
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	  UPDATE [dbo].product	
   SET	
   productname=dbo.EmptyStrToNull(i.productname)   ,
				 companyid=i.companyid,producttypeid=i.producttypeid,
      Description=dbo.EmptyStrToNull(i.description)
     

 from inserted	as i
 WHERE product.productid=i.productid

					;



			 
END