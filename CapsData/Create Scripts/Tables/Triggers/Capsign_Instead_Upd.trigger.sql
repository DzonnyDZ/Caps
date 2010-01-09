CREATE TRIGGER [dbo].[Capsign_Instead_Upd]
   ON  [dbo].[CapSign] 
   instead of update
AS 
BEGIN
	SET NOCOUNT ON;
	  UPDATE [dbo].CapSign
   SET	
			name=i.name,
      Description=dbo.EmptyStrToNull(i.description)
 from inserted	as i
 WHERE Capsign.capsignid=i.capsignid;
END