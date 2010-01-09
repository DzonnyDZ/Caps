
CREATE TRIGGER dbo.Company_Instead_Ins 
   ON  dbo.Company 
   instead of INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
			 insert into dbo.company
					 (	CompanyName,[Description])
             output inserted.*
			 SELECT dbo.EmptyStrToNull(CompanyName),dbo.EmptyStrToNull([Description])
  FROM inserted	 ;

  --  select * from dbo.company where CompanyID=scope_identity();

			 
END
