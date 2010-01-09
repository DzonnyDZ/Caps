


CREATE TRIGGER [dbo].[Target_Instead_Upd]
   ON  [dbo].[Target] 
   instead of update
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	  UPDATE [dbo].target
   SET	
   --name=dbo.EmptyStrToNull(i.name)   ,
			name=i.name,
      Description=dbo.EmptyStrToNull(i.description)

     

 from inserted	as i
 WHERE target.targetid=i.targetid

					;



			 
END


