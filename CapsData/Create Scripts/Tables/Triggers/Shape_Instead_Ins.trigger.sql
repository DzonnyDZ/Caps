
CREATE TRIGGER dbo.Shape_Instead_Ins 
   ON  dbo.Shape
   instead of INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
			 insert into dbo.Shape
					 (	 Name,Size1Name,Size2Name,[Description])
         output inserted.*
			 SELECT dbo.EmptyStrToNull(Name),dbo.EmptyStrToNull(Size1Name),	dbo.EmptyStrToNull(Size2Name),
			 dbo.EmptyStrToNull([Description])
  FROM inserted	 ;


     -- select * from dbo.shape where shapeid=scope_identity();
			 
END
