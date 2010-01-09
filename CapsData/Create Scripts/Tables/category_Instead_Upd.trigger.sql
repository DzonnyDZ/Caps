


CREATE TRIGGER [dbo].category_Instead_Upd
   ON  [dbo].category 
   instead of update
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	  UPDATE [dbo].category	
   SET	
   categoryName=dbo.EmptyStrToNull(i.categoryName),
   
      Description=dbo.EmptyStrToNull(i.description)

 from inserted	as i
 WHERE category.categoryid=i.categoryid

					;



			 
END