


CREATE TRIGGER [dbo].storagetype_Instead_Upd
   ON  [dbo].storagetype 
   instead of update
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	  UPDATE [dbo].storagetype	
   SET	
   name=dbo.EmptyStrToNull(i.name)   ,
			
      Description=dbo.EmptyStrToNull(i.description)
      
     

 from inserted	as i
 WHERE storagetype.storagetypeid=i.storagetypeid

					;



			 
END


