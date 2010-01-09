
CREATE TRIGGER dbo.StorageType_Instead_Ins 
   ON  dbo.StorageType
   instead of INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
			 insert into dbo.StorageType
					 (	 Name,[Description])
        output inserted.*
			 SELECT dbo.EmptyStrToNull(Name),dbo.EmptyStrToNull([Description])
  FROM inserted	 ;


    --  select * from dbo.storagetype where storagetypeid=scope_identity();
			 
END
