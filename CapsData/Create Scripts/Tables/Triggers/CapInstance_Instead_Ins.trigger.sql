
create TRIGGER dbo.CapInstance_Instead_Ins 
   ON  dbo.CapInstance
   instead of INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
			 insert into dbo.CapInstance
					 (	 CapID,Storageid,state,year,countrycode,datecreated,note)
         output inserted.*
			 SELECT capid,storageid,state,year,dbo.EmptyStrToNull(countrycode),isnull(datecreated,getdate()),dbo.EmptyStrToNull(note)
  FROM inserted	 ;

   --  select * from dbo.producttype where producttypeid=scope_identity();

			 
END
