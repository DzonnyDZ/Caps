

CREATE TRIGGER [dbo].[CapType_Instead_Ins] 
   ON  [dbo].[CapType] 
   instead of INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
			 insert into dbo.captype
					 (	 [TypeName]
      ,[ShapeID]
      ,[Size]
      ,[Size2]
      ,[Height]
      ,[MaterialID]
      ,[MainTypeID]
      ,[Description],TargetID)
                     output inserted.*
			 SELECT dbo.EmptyStrToNull([TypeName])
      ,[ShapeID]
      ,[Size]
      ,[Size2]
      ,[Height]
      ,[MaterialID]
      ,[MainTypeID]
      ,dbo.EmptyStrToNull([Description]),TargetID
  FROM inserted	 ;

     --   select * from dbo.captype where captypeid=scope_identity();

			 
END

