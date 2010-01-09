


CREATE TRIGGER [dbo].MainType_Instead_Upd
   ON  [dbo].MainType 
   instead of update
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	  UPDATE [dbo].MainType	
   SET	
   typename=dbo.EmptyStrToNull(i.typename)   ,
   
      Description=dbo.EmptyStrToNull(i.description)
     

 from inserted	as i
 WHERE MainType.MainTypeID=i.MainTypeID

					;



			 
END


