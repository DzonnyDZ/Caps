
CREATE TRIGGER dbo.Storage_Instead_Ins 
   ON  dbo.Storage
   instead of INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
			 insert into dbo.Storage
					 (	 StorageNumber,[Description],StorageTypeID)
          output inserted.*
			 SELECT StorageNumber,dbo.EmptyStrToNull([Description]),StorageTypeID
  FROM inserted	 ;


    --   select * from dbo.storage where storageid=scope_identity();
			 
END
