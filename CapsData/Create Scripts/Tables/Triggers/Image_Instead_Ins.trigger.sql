
CREATE TRIGGER dbo.Image_Instead_Ins 
   ON  dbo.[Image]
   instead of INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
			 insert into dbo.[image]
					 (	RelativePath,CapID,IsMain)
           output inserted.*
			 SELECT dbo.EmptyStrToNull(RelativePath),CapID,IsMain
  FROM inserted	 ;


  --   select * from dbo.image where imageid=scope_identity();
			 
END
