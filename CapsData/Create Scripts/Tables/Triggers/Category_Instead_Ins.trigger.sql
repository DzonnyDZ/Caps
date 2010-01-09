
CREATE TRIGGER dbo.Category_Instead_Ins 
   ON  dbo.Category 
   instead of INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
			 insert into dbo.category
					 (	 CategoryName,[Description])
               output inserted.*
			 SELECT dbo.EmptyStrToNull(CategoryName),dbo.EmptyStrToNull([Description])
  FROM inserted	 ;

 --  select * from dbo.category where categoryid=scope_identity();

			 
END
