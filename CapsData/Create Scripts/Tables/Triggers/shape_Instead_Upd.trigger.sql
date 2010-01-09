


CREATE TRIGGER [dbo].shape_Instead_Upd
   ON  [dbo].shape 
   instead of update
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	  UPDATE [dbo].shape	
   SET	
   name=dbo.EmptyStrToNull(i.name)   ,
				size1name=dbo.EmptyStrToNull(i.size1name)   ,
				size2name=dbo.EmptyStrToNull(i.size2name)   , 
      Description=dbo.EmptyStrToNull(i.description)
     

 from inserted	as i
 WHERE shape.shapeid=i.shapeid

					;



			 
END