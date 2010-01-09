
create TRIGGER [dbo].[Target_Instead_Ins] 
   ON  [dbo].[Target]
   instead of INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
			 insert into dbo.Target
					 (	 Name,[Description])
          output inserted.*
			 SELECT Name,dbo.EmptyStrToNull([Description])
  FROM inserted	 ;


    --   select * from dbo.storage where storageid=scope_identity();
			 
END
