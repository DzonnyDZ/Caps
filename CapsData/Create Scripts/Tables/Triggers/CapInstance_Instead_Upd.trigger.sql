
create TRIGGER dbo.CapInstance_Instead_Upd
   ON  dbo.CapInstance
   instead of update
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
			 update dbo.CapInstance
					 
         set
			 capid=i.capid,
			 storageid=i.storageid,
			 state=i.state,year=i.year,
			 countrycode=dbo.EmptyStrToNull(i.countrycode),
			 note=dbo.EmptyStrToNull(i.note)
  FROM inserted	i ;

   --  select * from dbo.producttype where producttypeid=scope_identity();

			 
END
