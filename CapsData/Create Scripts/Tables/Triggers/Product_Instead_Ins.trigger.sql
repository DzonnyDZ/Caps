
CREATE TRIGGER dbo.Product_Instead_Ins 
   ON  dbo.Product
   instead of INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
			 insert into dbo.Product
					 (	 ProductName,CompanyID,ProductTypeID,[Description])
 output inserted.*
			 SELECT dbo.EmptyStrToNull(ProductName),CompanyID,ProductTypeID,dbo.EmptyStrToNull([Description])
  FROM inserted	 ;
 --   select * from dbo.product where productid=scope_identity();


			 
END
