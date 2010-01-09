
CREATE TRIGGER dbo.ProductType_Instead_Ins 
   ON  dbo.ProductType
   instead of INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
			 insert into dbo.ProductType
					 (	 ProductTypeName,[Description],IsDrink,IsAlcoholic)
         output inserted.*
			 SELECT dbo.EmptyStrToNull(ProductTypeName),dbo.EmptyStrToNull([Description]),IsDrink,IsAlcoholic
  FROM inserted	 ;

   --  select * from dbo.producttype where producttypeid=scope_identity();

			 
END
