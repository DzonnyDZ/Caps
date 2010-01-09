CREATE TRIGGER [dbo].image_Instead_Upd
   ON  [dbo].image 
   instead of update
AS 
BEGIN
	SET NOCOUNT ON;
	  UPDATE [dbo].image	
   SET	
   relativepath=dbo.EmptyStrToNull(i.relativepath),
      capid=i.capid,IsMain = i.IsMain

 from inserted	as i
 WHERE image.imageid=i.imageid;
		 
END;


