
CREATE TRIGGER dbo.Material_Instead_Ins 
   ON  dbo.Material
   instead of INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
			 insert into dbo.Material
					 (	 Name,[Description])
        output inserted.*
			 SELECT dbo.EmptyStrToNull(Name),dbo.EmptyStrToNull([Description])
  FROM inserted	 ;


   --  select * from dbo.material where materialid=scope_identity();
			 
END
