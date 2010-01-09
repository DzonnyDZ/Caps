


CREATE TRIGGER [dbo].[Material_Instead_Upd]
   ON  [dbo].material 
   instead of update
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	  UPDATE [dbo].material	
   SET	
   name=dbo.EmptyStrToNull(i.name)   ,
   
      Description=dbo.EmptyStrToNull(i.description)
     

 from inserted	as i
 WHERE material.materialid=i.materialid

					;



			 
END