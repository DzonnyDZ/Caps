


CREATE TRIGGER [dbo].storage_Instead_Upd
   ON  [dbo].storage 
   instead of update
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	  UPDATE [dbo].storage	
   SET	
   --name=dbo.EmptyStrToNull(i.name)   ,
			storagenumber=i.storagenumber,
      Description=dbo.EmptyStrToNull(i.description),
      storagetypeid=i.storagetypeid
     

 from inserted	as i
 WHERE storage.storageid=i.storageid

					;



			 
END


