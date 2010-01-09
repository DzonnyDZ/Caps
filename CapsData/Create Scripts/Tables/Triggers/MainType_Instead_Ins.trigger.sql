
CREATE TRIGGER dbo.MainType_Instead_Ins 
   ON  dbo.MainType
   instead of INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
			 insert into dbo.MainType
					 (	 TypeName,[Description])
        output inserted.*
			 SELECT dbo.EmptyStrToNull(TypeName),dbo.EmptyStrToNull([Description])
  FROM inserted	 ;


 --  select * from dbo.maintype where maintypeid=scope_identity();
			 
END
