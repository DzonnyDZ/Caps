CREATE TRIGGER [dbo].[PseudoCategory_Instead_Upd]
   ON  [dbo].PseudoCategory 
   instead of update
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	  UPDATE [dbo].PseudoCategory	
   SET	
   name=dbo.EmptyStrToNull(i.name)   ,
      [Description]=dbo.EmptyStrToNull(i.[description]),
      Condition=dbo.EmptyStrToNull(i.Condition)
     

 from inserted	as i
 WHERE pseudocategory.pseudocategoryid=i.pseudocategoryid

					;



			 
END


